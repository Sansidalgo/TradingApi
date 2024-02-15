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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        DateTime currentTime = DateTime.Now;
        Logger logger = LogManager.GetCurrentClassLogger();
        public OrderRepository(AlgoContext _context) : base(_context)
        {
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            TimeSpan startTime = DateTime.Now.TimeOfDay;
            TimeSpan endTime = startTime;
            currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, istTimeZone);

        }

        public async Task<DbStatus> PlacePaperSellOrder(OrderSettingsResponseDto order, ShoonyaReponseDto shoonyaResponse)
        {
            logger.Info($"User Id : {order.Trader.Id} : Started Selling Paper Order");
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
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Status = 0;
                res.Message = res.GetStatus(ex);


            }


            if (res.Status == 0)
            {
                await CommonHelper.InfoAsync("Ending Selling Paper Order with Error", logger);

            }
            else
            {
                await CommonHelper.InfoAsync("Performed Selling Paper Order Successfully", logger);
            }

            return res;
        }

        public async Task<DbStatus> PlacePaperBuyOrder(bool placeNewOrder, OrderSettingsResponseDto order, string asset, decimal IndexPrice, ShoonyaReponseDto shoonyaResponse)
        {
            await CommonHelper.InfoAsync($"User ID: {order.Trader.Id} Started Buying Paper Order with Error", logger);
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
                    await CommonHelper.LogExceptionAsync(ex, logger);
                    res.Message = res.GetStatus(ex);


                }

            }
            if (res.Status == 0)
            {
                await CommonHelper.InfoAsync("Ending Buying Paper Order with Error", logger);

            }
            else
            {
                await CommonHelper.InfoAsync("Performed Buying Paper Order Successfully", logger);
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
            await CommonHelper.InfoAsync($"User ID: {order.Trader.Id}: Started Selling Order", logger);
            DbStatus res = new DbStatus();
            StringBuilder status = new StringBuilder();
            try
            {


                string loggedInUser = string.Empty;

                var nApi = shoonyaResponse.NorenRestApi;
                var responseHandler = shoonyaResponse.BaseResponseHandler;


                res.Status = 1;
                await CommonHelper.InfoAsync($"Order Side: {order.OrderSide.Name}", logger);
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
                                    placeOrder.trantype = "N";
                                    placeOrder.prc = "0";
                                    placeOrder.prctyp = "MKT";
                                    placeOrder.ret = "DAY";
                                    placeOrder.ordersource = "API";
                                    string indexName = string.Empty;
                                    if (order.OptionsSetting.Instrument.Name.ToUpper() == "NIFTY 50")
                                    {
                                        indexName = "NIFTY";
                                    }
                                    else
                                    {
                                        indexName = order.OptionsSetting.Instrument.Name.ToUpper();
                                    }


                                    if (p.tsym.ToUpper().StartsWith(indexName) && p.tsym.Substring(p.tsym.Length - 8).Contains("P") && (order.OrderSide.Name.ToLower() == "pesell" || order.OrderSide.Name.ToLower() == "cebuy"))
                                    {

                                        placeOrder.trantype = "S";

                                    }
                                    else if (p.tsym.ToUpper().StartsWith(indexName) && p.tsym.Substring(p.tsym.Length - 8).Contains("C") && (order.OrderSide.Name.ToLower() == "cesell" || order.OrderSide.Name.ToLower() == "pebuy"))
                                    {


                                        placeOrder.trantype = "S";
                                    }

                                    if (placeOrder.trantype == "S")
                                    {
                                        await nApi.SendPlaceOrderAsync(responseHandler.OnResponse, placeOrder);

                                        await CommonHelper.InfoAsync($"Successfully placed sell live order: {responseHandler?.baseResponse?.toJson()}", logger);

                                        res.Status = 1;

                                        Console.WriteLine("app handler :" + responseHandler?.baseResponse?.toJson());
                                    }


                                }
                                catch (Exception ex)
                                {
                                    res.Status = 0;
                                    res.Message = ex.Message;
                                    await CommonHelper.LogExceptionAsync(ex, logger);

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
                await CommonHelper.InfoAsync(ex.Message, logger);
                res.Status = 0;
                status.Append(ex.StackTrace);
                status.Append(Environment.NewLine);

            }
            res.Message = status.ToString();
            if (res.Status == 0)
            {
                await CommonHelper.InfoAsync("Ending Selling Order with Error", logger);

            }
            else
            {
                await CommonHelper.InfoAsync("Performed Selling Order Successfully", logger);
            }
            return res;
        }
        public async Task<DbStatus> PlaceBuyOrderLive(bool placeNewOrder, string asset, OrderSettingsResponseDto order, decimal IndexPrice, ShoonyaReponseDto shoonyaResponse)
        {
            await CommonHelper.InfoAsync($"User ID: {order.Trader.Id}: Started Buying Order ", logger);

            int retry = 0;
        lblRetry:
            DbStatus res = new DbStatus();
            string loggedInUser = string.Empty;
            StringBuilder status = new StringBuilder();
            var nApi = shoonyaResponse.NorenRestApi;
            var responseHandler = shoonyaResponse.BaseResponseHandler;
            PlaceOrder placeOrder;
            res.Status = 1;
            try
            {
                await CommonHelper.InfoAsync($"Order Side: {order.OrderSide.Name}", logger);
                if (placeNewOrder && order.OrderSide.Name.ToLower().Contains("buy"))
                {
                    placeOrder = new PlaceOrder();
                    placeOrder.uid = CommonHelper.DecodeValue(order.Credential.Uid);
                    placeOrder.actid = CommonHelper.DecodeValue(order.Credential.Uid);
                    placeOrder.exch = order.OptionsSetting.Exchange;
                    placeOrder.tsym = asset;
                    placeOrder.qty = Convert.ToString(order.OptionsSetting.PlayQuantity);
                    placeOrder.dscqty = "0";
                    placeOrder.prd = "M";
                    if (order.OrderSide.Name.ToLower().Contains("buy"))
                    {
                        placeOrder.trantype = "B";
                    }

                    placeOrder.prc = "0";
                    placeOrder.prctyp = "MKT";
                    placeOrder.ret = "DAY";
                    placeOrder.ordersource = "API";
                    placeOrder.remarks = "";

                    await CommonHelper.InfoAsync($"Order Details: UID: {order.Credential.Uid}, actID: {order.Credential.Uid} exch: {order.OptionsSetting.Exchange} tsym:{placeOrder.tsym} qty: {placeOrder.qty} ", logger);

                    if (order.OrderSide.Name.ToLower().Contains("buy"))
                    {
                        await nApi.SendGetOrderBookAsync(responseHandler.OnResponse, placeOrder.tsym);
                        var orderBookBefore = responseHandler.baseResponse as OrderBookResponse;


                        await CommonHelper.InfoAsync("Before send buy order", logger);
                        await nApi.SendPlaceOrderAsync(responseHandler.OnResponse, placeOrder);

                        var orderPlaceResponse = responseHandler.baseResponse as PlaceOrderResponse;

                        await nApi.SendGetOrderBookAsync(responseHandler.OnResponse, placeOrder.tsym);
                        var orderBookAfter = responseHandler.baseResponse as OrderBookResponse;
                        if (orderBookBefore.list == null && orderBookAfter.list == null )
                        {
                            if (retry < 2)
                            {
                                await CommonHelper.InfoAsync($"Retrying buy order", logger);
                                retry++;
                                goto lblRetry;
                            }
                        }
                        else if(orderBookAfter.list.Count==0)
                        {
                            if (retry < 2)
                            {
                                await CommonHelper.InfoAsync($"Retrying buy order", logger);
                                retry++;
                                goto lblRetry;
                            }

                        }
                         if (orderBookBefore.list != null && orderBookAfter.list != null && orderBookBefore.list.Count != orderBookAfter.list.Count)
                        {
                            await CommonHelper.InfoAsync($"successfully placed live buy order: {orderPlaceResponse?.toJson()}", logger);

                            placeNewOrder = true;
                            res.Status = 1;

                        }
                        else if (orderBookAfter.list != null && orderBookAfter.list.Count > 0)
                        {
                            await CommonHelper.InfoAsync($"reject reason: {orderBookAfter.list.FirstOrDefault().rejreason} remarks: {orderBookAfter.list.FirstOrDefault().remarks}", logger);
                        }
                        else if (orderBookBefore.list != null && orderBookAfter.list != null && orderBookBefore.list.Count == orderBookAfter.list.Count)
                        {
                            if (retry < 2)
                            {
                                await CommonHelper.InfoAsync($"Retrying buy order", logger);
                                retry++;
                                goto lblRetry;
                            }
                        }
                        else
                        {
                            if (retry < 2)
                            {
                                await CommonHelper.InfoAsync($"Retrying buy order", logger);
                                retry++;
                                goto lblRetry;
                            }
                        }

                    }

                    Console.WriteLine("app handler :" + responseHandler?.baseResponse?.toJson());
                    //return string.Empty;
                }



            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("Value cannot be null"))
                {
                    if (retry < 2)
                    {
                        retry++;
                        goto lblRetry;
                    }

                }
                await CommonHelper.InfoAsync(ex.Message, logger);
                //_logger.LogInformation("Error: " + ex.StackTrace);
                status.Append(loggedInUser + ex.StackTrace + " Exits Orders: ");
                res.Status = 0;
            }
            res.Message = status.ToString();
            if (res.Status == 0)
            {
                await CommonHelper.InfoAsync("Ended Buying Order with Error", logger);
            }
            else
            {
                await CommonHelper.InfoAsync("Successfully completed byuing order", logger);
            }
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
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
        public async Task<DbStatus> GetOptionChainAsync(ShoonyaReponseDto shoonyaResponse, string exchange, string tsym, string strikePrice, int count, string orderSide)
        {
            DbStatus res = new DbStatus();

            try
            {
                res.Result = 0;

                string loggedInUser = string.Empty;
                StringBuilder status = new StringBuilder();
                var nApi = shoonyaResponse.NorenRestApi;
                var responseHandler = shoonyaResponse.BaseResponseHandler;
                await nApi.SendGetOptionChainAsync(responseHandler.OnResponse, exchange.ToUpper(), tsym, strikePrice, count);
                var response = responseHandler.baseResponse as OptionChainResponse;
                List<OptionContractResponseDto> nifty50Options = new List<OptionContractResponseDto>();
                foreach (OptionChainItem item in response.values)
                {
                    var dta11 = await nApi.SendSearchScripAsync(responseHandler.OnResponse, exchange.ToUpper(), item.tsym);
                    var oc1 = responseHandler.baseResponse as SearchScripResponse;


                    var dta = await nApi.SendGetQuoteAsync(responseHandler.OnResponse, exchange.ToUpper(), item.token);
                    var oc = responseHandler.baseResponse as GetQuoteResponse;
                    OptionContractResponseDto ocrd = new OptionContractResponseDto();
                    ocrd.OptionType = item.optt;

                    ocrd.StrikePrice = Convert.ToDouble(item.strprc);
                    ocrd.OpenInterest = Convert.ToDouble(oc.oi);
                    ocrd.Symbol = oc.tsym;
                    ocrd.DailyTradingRange = Convert.ToDouble(oc.h) - Convert.ToDouble(oc.l);
                    nifty50Options.Add(ocrd);

                }
                string side = string.Empty;
                if (orderSide.ToUpper().Contains("PE"))
                {
                    side = "PE";
                }
                else
                {
                    side = "CE";
                }
                res.Result = OptionContractAnalyzerRepository.GetOptionContract(nifty50Options, side);
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
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

                //await nApi.SendSearchScripAsync(responseHandler.OnResponse, exchange.ToUpper(), "NIFTY25JAN24F");
                //var responseScript = responseHandler.baseResponse as SearchScripResponse;

                await nApi.SendGetIndexListAsync(responseHandler.OnResponse, exchange.ToUpper());
                var response = responseHandler.baseResponse as GetIndexListResponse;
                if (response.values != null)
                {
                    var token = response.values.Where(w => w.idxname.ToUpper() == scrip.ToUpper()).Select(s => s.token).FirstOrDefault();
                    var res1 = await nApi.SendGetQuoteAsync(responseHandler.OnResponse, exchange.ToUpper(), token);
                    var response1 = responseHandler.baseResponse as GetQuoteResponse;
                    res.Result = response1.lp;

                }


                res.Status = 1;
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
    }
}
