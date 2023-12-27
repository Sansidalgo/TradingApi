using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(AlgoContext _context) : base(_context)
        {
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
                if(order.OrderSide.Name=="pesell" || order.OrderSide.Name == "cesell")
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

                    orderResponse.CreatedDt = DateTime.Now;
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
            try
            {


                string loggedInUser = string.Empty;
                StringBuilder status = new StringBuilder();
                var nApi = shoonyaResponse.NorenRestApi;
                var responseHandler = shoonyaResponse.BaseResponseHandler;
                res.Status = 1;
                if (!order.Environment.Name.ToLower().Equals("papertrading"))
                {

                    var openOrders = await nApi.SendGetPositionBookAsync(responseHandler.OnResponse, CommonHelper.DecodeValue(order.Credential.Uid));

                    if (responseHandler.baseResponse != null)
                    {
                        var bookResponse = responseHandler.baseResponse as PositionBookResponse;
                        if (bookResponse != null && (bookResponse.stat != "Not_Ok" && Convert.ToString(bookResponse.stat).ToLower() != "unauthorized" ) && bookResponse.emsg!=null && !bookResponse.emsg.ToLower().Contains("error"))
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


                                    if (p.tsym.ToUpper().StartsWith(order.OptionsSetting.Instrument.Name.ToUpper()) && p.tsym.Substring(p.tsym.Length - 8).Contains("P") && (order.OrderSide.Name == "pesell" || order.OrderSide.Name == "cebuy"))
                                    {

                                        placeOrder.trantype = "S";

                                    }
                                    else if (p.tsym.ToUpper().StartsWith(order.OptionsSetting.Instrument.Name.ToUpper()) && p.tsym.Substring(p.tsym.Length - 8).Contains("C") && (order.OrderSide.Name == "cesell" || order.OrderSide.Name == "pebuy"))
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

                                        status.Append("Successfully placed exit order" + responseHandler?.baseResponse?.toJson());
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
                res.Message = ex.Message;

            }
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
                        SellAt=s.SellAt,
                        Quantity = s.Quantity,
                        IndexPriceAt = s.IndexPriceAt,
                        StrategyName = s.OrderSettings.Strategy.Name,
                        OrderSideName = s.OrderSettings.OrderSide.Name,
                        SegmentName = s.Segment.Name,
                        EnvironmentName = s.OrderSettings.Environment.Name,
                        OrderSourceName = s.OrderSource.Name,
                        InstrumentName = s.OrderSettings.OptionsSettings.Instrument.Name
                    })
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
    }
}
