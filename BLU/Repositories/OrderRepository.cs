using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        DateTime currentTime = DateTime.Now;
        public OrderRepository(AlgoContext _context) : base(_context)
        {
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            TimeSpan startTime = DateTime.Now.TimeOfDay;
            TimeSpan endTime = startTime;
            currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, istTimeZone);

        }

        public async Task<DbStatus> ExecuteOrderLogic(ShoonyaOrder order, OrderSettingsRepository settingsRepo, ShoonyaCredentialsRepository shoonyaCredentialsRepo)
        {
            var logger = LogManager.GetCurrentClassLogger();

            // Create a new Stopwatch instance for each user
            Stopwatch stopwatch = Stopwatch.StartNew();

            OrderSettingsResponseDto orderSettings;
            DbStatus res = new DbStatus();
            logger.Info("Getting Order Settings details");

            try
            {
                res = await settingsRepo.GetOrderSettingsById(CommonHelper.GetNumberFromString(order.OSID));
                logger.Info(res.Message);
                orderSettings = (OrderSettingsResponseDto)res.Result;
                string user = string.Empty;

                if (!string.IsNullOrWhiteSpace(Convert.ToString(orderSettings.TraderId)))
                {
                    logger.Info("Order details received from db");
                    user = $" User Id: {CommonHelper.DecodeValue(orderSettings.Credential.Uid)} Shoonya ID: {orderSettings?.TraderId} ";

                }
                else
                {
                    logger.Info("Order Setting does not exist");
                    return res;
                }

                res = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
                logger.Info($"{user}{res.Message}");
                ShoonyaReponseDto shoonyaResponse = res.Result as ShoonyaReponseDto;
                if (order.IndexPrice <= 0)
                {
                    res = await GetIndex(shoonyaResponse, orderSettings.OptionsSetting.Instrument.Exchange, orderSettings.OptionsSetting.Instrument.Name);
                    order.IndexPrice = Convert.ToDecimal(res.Result);

                }
                if (res.Status == 1)
                {
                    // Placeholder for the rest of your code
                    // Add your specific logic here, e.g., additional API calls, data processing, etc.

                    // Example: Perform some additional processing
                    string asset = await CommonHelper.GetFOAsset(orderSettings.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSettings.OptionsSetting.CeSideEntryAt), order.IndexPrice, orderSettings.OrderSide.Name, orderSettings.OptionsSetting.Instrument.ExpiryDay);
                    logger.Info($"{user}asset: {asset}");

                    if (!(await CheckWhetherInTimeWindow(orderSettings.OptionsSetting.StartTime, orderSettings.OptionsSetting.StartTime)))
                    {
                        res.Message = "Not in time window";
                        logger.Info($"{user}{res.Message}");
                        res.Status = 0;
                        return res;
                    }

                    if (!orderSettings.Environment.Name.ToLower().Equals("papertrading"))
                    {
                        logger.Info($"{user}Started Selling Live Order");
                        res = await PlaceSellOrderLive(orderSettings, shoonyaResponse);
                        logger.Info($"{user}{res.Message}");
                        logger.Info($"{user}Completed Selling Live Order");

                        logger.Info($"{user}Started Buying Live Order");
                        res = await PlaceBuyOrderLive(Convert.ToBoolean(res.Status), asset, orderSettings, order.IndexPrice, shoonyaResponse);
                        logger.Info($"{user}{res.Message}");
                        logger.Info($"{user}Completed Buying Live Order");
                    }
                    else
                    {
                        logger.Info($"{user}Started Selling Paper Order");
                        res = await PlacePaperSellOrder(orderSettings, shoonyaResponse);
                        logger.Info($"{user}{res.Message}");
                        logger.Info($"{user}Completed Selling Paper Order");

                        logger.Info($"{user}Started Buying Paper Order");
                        res = await PlacePaperBuyOrder(Convert.ToBoolean(res.Status), orderSettings, asset, order.IndexPrice, shoonyaResponse);
                        logger.Info($"{user}{res.Message}");
                        logger.Info($"{user}Completed Buying Paper Order");
                    }

                    logger.Info($"Shoonya User: {CommonHelper.DecodeValue(orderSettings.Credential.Uid)}");
                    logger.Info($"{user}{res.Message}");

                    // End of the placeholder

                    stopwatch.Stop();
                    TimeSpan elapsedTime = stopwatch.Elapsed;
                    logger.Info($"API method execution time for {user}: {elapsedTime.TotalMilliseconds} milliseconds");

                    return res;
                }
                else
                {
                    logger.Info($"{user}{res.Message}");
                    return res;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                logger.Info($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception after logging or handling
            }
        }

        public async Task<DbStatus> PlacePaperSellOrder(OrderSettingsResponseDto order, ShoonyaReponseDto shoonyaResponse)
        {
            DbStatus res = new DbStatus();

            var nApi = shoonyaResponse.NorenRestApi;
            var responseHandler = shoonyaResponse.BaseResponseHandler;
            res.Status = 1;
            try
            {
                var statusId = (await context.TblStatusTypes.Where(w => w.Name == "open").FirstOrDefaultAsync()).Id; ;
                var orders = context.TblOrders.Where(w => w.TraderId == order.Trader.Id && w.StatusId == statusId).ToList();

                if (orders.Count > 0)
                {
                    statusId = (await context.TblStatusTypes.Where(w => w.Name == "close").FirstOrDefaultAsync()).Id;
                    foreach (var item in orders)
                    {
                        await nApi.SendSearchScripAsync(responseHandler.OnResponse, order.OptionsSetting.Exchange, item.Asset);
                        var scripSearchResponse = responseHandler.baseResponse as SearchScripResponse;
                        await nApi.SendGetQuoteAsync(responseHandler.OnResponse, order.OptionsSetting.Exchange, scripSearchResponse.values.FirstOrDefault().token);
                        var quoteResponse = responseHandler.baseResponse as GetQuoteResponse;
                        item.SellAt = quoteResponse.lp != null ? Convert.ToDecimal(quoteResponse.lp) : (decimal?)null;

                        item.StatusId = statusId;

                    }
                    context.TblOrders.UpdateRange(orders);
                    await context.SaveChangesAsync();
                }
                else
                {
                    res.Status = 1;
                }
                if (order.OrderSide.Name == "pesell" || order.OrderSide.Name == "cesell")
                {
                    res.Status = 0;
                }

            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);


            }




            return res;



        }

        public async Task<DbStatus> PlacePaperBuyOrder(bool placeNewOrder, OrderSettingsResponseDto order, string asset, decimal IndexPrice, ShoonyaReponseDto shoonyaResponse)
        {
            DbStatus res = new DbStatus();

            if (placeNewOrder)
            {
                try
                {


                    var nApi = shoonyaResponse.NorenRestApi;
                    var responseHandler = shoonyaResponse.BaseResponseHandler;
                    TblOrder orderResponse = new TblOrder();
                    orderResponse.Asset = asset;
                    orderResponse.IndexPriceAt = IndexPrice;

                    orderResponse.CreatedDt = currentTime;
                    orderResponse.TraderId = order.Trader.Id;
                    orderResponse.Quantity = order.OptionsSetting.PlayQuantity;
                    orderResponse.EnvironmentId = order.Environment.Id;
                    orderResponse.OrderSettingsId = order.Id;

                    await nApi.SendSearchScripAsync(responseHandler.OnResponse, order.OptionsSetting.Exchange, asset);
                    var scripSearchResponse = responseHandler.baseResponse as SearchScripResponse;
                    await nApi.SendGetQuoteAsync(responseHandler.OnResponse, order.OptionsSetting.Exchange, scripSearchResponse.values.FirstOrDefault().token);
                    var quoteResponse = responseHandler.baseResponse as GetQuoteResponse;
                    orderResponse.BuyAt = quoteResponse.lp != null ? Convert.ToDecimal(quoteResponse.lp) : (decimal?)null;

                    orderResponse.StatusId = (await context.TblStatusTypes.Where(w => w.Name == "open").FirstOrDefaultAsync()).Id;
                    orderResponse.SegmentId = (await context.TblSegments.Where(w => w.Name == "options").FirstOrDefaultAsync()).Id;
                    orderResponse.OrderSourceId = (await context.TblOrderSources.Where(w => w.Name == "auto").FirstOrDefaultAsync()).Id;
                    orderResponse.OrderSideId = order.OrderSide.Id;




                    await context.TblOrders.AddAsync(orderResponse);

                    res.Status = await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    res.Message = res.GetStatus(ex);


                }

            }


            return res;



        }
        public async Task<bool> CheckWhetherInTimeWindow(string startWindowTime, string endWindowTime)
        {
            // Create a TimeZoneInfo object for Indian Standard Time (IST)
            bool res = true;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            TimeSpan startTime = DateTime.Now.TimeOfDay;
            TimeSpan endTime = startTime;
            DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, istTimeZone);


            if (!string.IsNullOrWhiteSpace(startWindowTime) && !startWindowTime.Contains("0:0"))
            {
                startTime = TimeSpan.Parse(startWindowTime);
                if (!(currentTime.TimeOfDay < startTime))
                {
                    res = false;
                }

            }
            if (!string.IsNullOrWhiteSpace(endWindowTime) && !endWindowTime.Contains("0:0"))
            {
                endTime = TimeSpan.Parse(endWindowTime);
                if (!(currentTime.TimeOfDay > endTime))
                {
                    res = false;
                }

            }

            return await Task.FromResult(res);
            // Compare the current time with the start time


        }

        public async Task<DbStatus> PlaceSellOrderLive(OrderSettingsResponseDto order, ShoonyaReponseDto shoonyaResponse)
        {
            DbStatus res = new DbStatus();
            StringBuilder status = new StringBuilder();
            try
            {


                string loggedInUser = string.Empty;

                var nApi = shoonyaResponse.NorenRestApi;
                var responseHandler = shoonyaResponse.BaseResponseHandler;
                res.Status = 1;
                if (!order.Environment.Name.ToLower().Equals("papertrading"))
                {

                    var openOrders = await nApi.SendGetPositionBookAsync(responseHandler.OnResponse, CommonHelper.DecodeValue(order.Credential.Uid));

                    if (responseHandler.baseResponse != null)
                    {
                        var bookResponse = responseHandler.baseResponse as PositionBookResponse;
                        if (bookResponse != null && (bookResponse.stat != "Not_Ok" && Convert.ToString(bookResponse.stat).ToLower() != "unauthorized") && bookResponse.emsg != null && !bookResponse.emsg.ToLower().Contains("error"))
                        {
                            var openPositions = bookResponse?.positions.Where(x => Convert.ToInt32(x.netqty) > 0);
                            foreach (var p in openPositions)
                            {


                                try
                                {

                                    var placeOrder = new PlaceOrder();
                                    placeOrder.uid = CommonHelper.DecodeValue(order.Credential.Uid);
                                    placeOrder.actid = CommonHelper.DecodeValue(order.Credential.Uid);
                                    placeOrder.exch = order.OptionsSetting.Exchange;
                                    placeOrder.tsym = p.tsym;

                                    placeOrder.qty = p.netqty;
                                    placeOrder.dscqty = "0";
                                    placeOrder.prd = p.prd;

                                    placeOrder.prc = "0";
                                    placeOrder.prctyp = "MKT";
                                    placeOrder.ret = "DAY";
                                    placeOrder.ordersource = "API";
                                    string indexName = string.Empty;
                                    if (order.OptionsSetting.Instrument.Name.ToUpper()=="NIFTY 50")
                                    {
                                         indexName = "NIFTY";
                                    }
                                    else
                                    {
                                        indexName = order.OptionsSetting.Instrument.Name.ToUpper();
                                    }
                                    

                                    if (p.tsym.ToUpper().StartsWith(indexName) && p.tsym.Substring(p.tsym.Length - 8).Contains("P") && (order.OrderSide.Name == "pesell" || order.OrderSide.Name == "cebuy"))
                                    {

                                        placeOrder.trantype = "S";

                                    }
                                    else if (p.tsym.ToUpper().StartsWith(indexName) && p.tsym.Substring(p.tsym.Length - 8).Contains("C") && (order.OrderSide.Name == "cesell" || order.OrderSide.Name == "pebuy"))
                                    {


                                        placeOrder.trantype = "S";
                                    }
                                    else
                                    {
                                        res.Status = 0;
                                    }
                                    if (placeOrder.trantype == "S")
                                    {
                                        await nApi.SendPlaceOrderAsync(responseHandler.OnResponse, placeOrder);

                                        status.Append("Successfully placed sell live order" + responseHandler?.baseResponse?.toJson());
                                        status.Append('\n');
                                        res.Status = 1;

                                        Console.WriteLine("app handler :" + responseHandler?.baseResponse?.toJson());
                                    }


                                }
                                catch (Exception ex)
                                {
                                    //_logger.LogInformation("Error: " + ex.StackTrace);
                                    Console.WriteLine(ex.StackTrace);

                                    status.Append(loggedInUser + " :" + ex.StackTrace);
                                    status.Append('\n');

                                }


                            }
                        }
                        else
                        {
                            res.Status = 1;
                            status = status.Append(" no records");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = 0;
                status.Append(ex.StackTrace);
                status.Append(Environment.NewLine);

            }
            res.Message = status.ToString();
            return res;
        }
        public async Task<DbStatus> PlaceBuyOrderLive(bool placeNewOrder, string asset, OrderSettingsResponseDto order, decimal IndexPrice, ShoonyaReponseDto shoonyaResponse)
        {

            DbStatus res = new DbStatus();
            string loggedInUser = string.Empty;
            StringBuilder status = new StringBuilder();
            var nApi = shoonyaResponse.NorenRestApi;
            var responseHandler = shoonyaResponse.BaseResponseHandler;
            PlaceOrder placeOrder;
            res.Status = 1;
            try
            {
                if (placeNewOrder && order.OrderSide.Name.Contains("buy"))
                {
                    placeOrder = new PlaceOrder();
                    placeOrder.uid = CommonHelper.DecodeValue(order.Credential.Uid);
                    placeOrder.actid = CommonHelper.DecodeValue(order.Credential.Uid);
                    placeOrder.exch = order.OptionsSetting.Exchange;
                    placeOrder.tsym = asset;
                    status.Append("Prepared Asset: " + Convert.ToString(placeOrder.tsym));

                    placeOrder.qty = Convert.ToString(order.OptionsSetting.PlayQuantity);
                    placeOrder.dscqty = "0";
                    placeOrder.prd = "M";
                    if (order.OrderSide.Name.Contains("buy"))
                    {
                        placeOrder.trantype = "B";
                    }

                    placeOrder.prc = "0";
                    placeOrder.prctyp = "MKT";
                    placeOrder.ret = "DAY";
                    placeOrder.ordersource = "API";
                    placeOrder.remarks = "";
                    if (order.OrderSide.Name.Contains("buy"))
                    {


                        await nApi.SendPlaceOrderAsync(responseHandler.OnResponse, placeOrder);

                        var orderPlaceResponse = responseHandler.baseResponse as PlaceOrderResponse;

                        status.Append("successfully placed live buy order");
                        status.Append('\n');
                        status.Append(orderPlaceResponse?.toJson());
                        status.Append('\n');
                        placeNewOrder = true;
                        res.Status = 1;


                    }

                    Console.WriteLine("app handler :" + responseHandler?.baseResponse?.toJson());
                    //return string.Empty;
                }



            }
            catch (Exception ex)
            {
                //_logger.LogInformation("Error: " + ex.StackTrace);
                status.Append(loggedInUser + ex.StackTrace + " Exits Orders: ");
                res.Status = 0;
            }
            res.Message = status.ToString();
            return res;
        }

        public async Task<DbStatus> GetOrders(int traderID)
        {
            DbStatus res = new DbStatus();
            if (traderID == null) { return res; }
            try
            {

                var Result = await context.TblOrders.Where(w => w.TraderId == Convert.ToInt32(traderID))
                    .Select(s => new OrderResponseDto()
                    {
                        Id = s.Id,
                        CreatedDt = s.CreatedDt,
                        Asset = s.Asset,
                        BuyAt = s.BuyAt,
                        SellAt = s.SellAt,
                        Quantity = s.Quantity,
                        IndexPriceAt = s.IndexPriceAt,
                        StrategyName = s.OrderSettings.Strategy.Name,
                        OrderSideName = s.OrderSettings.OrderSide.Name,
                        SegmentName = s.Segment.Name,
                        EnvironmentName = s.OrderSettings.Environment.Name,
                        OrderSourceName = s.OrderSource.Name,
                        InstrumentName = s.OrderSettings.OptionsSettings.Instrument.Name
                    }).OrderByDescending(o => o.CreatedDt)
                    .ToListAsync();



                if (Result.Count > 0)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
        public async Task<DbStatus> GetIndex(ShoonyaReponseDto shoonyaResponse, string exchange, string scrip)
        {
            DbStatus res = new DbStatus();

            try
            {
                res.Result = 0;

                string loggedInUser = string.Empty;
                StringBuilder status = new StringBuilder();
                var nApi = shoonyaResponse.NorenRestApi;
                var responseHandler = shoonyaResponse.BaseResponseHandler;
                await nApi.SendGetIndexListAsync(responseHandler.OnResponse, exchange.ToUpper());
                var response = responseHandler.baseResponse as GetIndexListResponse;
                if(response.values!=null)
                {
                    var token = response.values.Where(w => w.idxname.ToUpper() == scrip.ToUpper()).Select(s => s.token).FirstOrDefault();
                    var res1 =await nApi.SendGetQuoteAsync(responseHandler.OnResponse, exchange.ToUpper(), token);
                    var response1 = responseHandler.baseResponse as GetQuoteResponse;
                    res.Result = response1.lp;
                    
                }
                

                res.Status = 1;
            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
    }
}
