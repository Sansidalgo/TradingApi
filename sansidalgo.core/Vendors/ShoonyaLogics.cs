using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sansidalgo.core.Vendors
{
    public class ShoonyaLogics : IShoonyaLogics
    {
        public static NorenRestApi? nApi;
        LoginMessage? loginMessage;

        private readonly BaseResponseHandler? responseHandler;
        private readonly CommonHelper helper;
        public ShoonyaLogics(CommonHelper _helper)
        {
            helper = _helper;
        }

        private static readonly string[] Summaries = new[]
      {
        "FA155912", "FA130431", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        public IEnumerable<WeatherForecast> GetOrders()
        {
           
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
             .ToArray();
        }

        public async Task<string> PostShoonyaOrder(Order order)
        {
            string loggedInUser = string.Empty;
            string status = string.Empty;

            PlaceOrder placeOrder; 
            try
            {
                var userId = await helper.DecodeValue(order.UID);
                if (!Summaries.Any(w => w.Equals(userId)))
                {
                    return await Task.FromResult(string.Concat("UID: ", order.UID, " is not registered"));
                }
                nApi = new NorenRestApi();
                var endPoint = "https://api.shoonya.com/NorenWClientTP/";
                LoginMessage loginMessage = new LoginMessage();
                loginMessage.apkversion = "1.0.0";
                loginMessage.uid = userId;
                loginMessage.pwd =await helper.DecodeValue(order?.PSW);
                loginMessage.vc =await helper.DecodeValue(order?.VC);
                loginMessage.factor2 =await helper.GetTOTP(order?.authSecretekey);
                loginMessage.appkey =order.ApiKey;
                loginMessage.imei = order.imei;
            
                loginMessage.source = "API";
           
                var responseHandler = new BaseResponseHandler();

                nApi.SendLogin(responseHandler.OnResponse, endPoint, loginMessage);

                responseHandler.ResponseEvent.WaitOne();

                LoginResponse? loginResponse = responseHandler?.baseResponse as LoginResponse;
                Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());
                //_logger.LogInformation("Logged in user:" + loginResponse.uname);
                loggedInUser = "User:" + loginResponse?.uname;

                // Create a TimeZoneInfo object for Indian Standard Time (IST)
                TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                TimeSpan startTime = TimeSpan.Parse(order.StartTime);
                TimeSpan endTime = TimeSpan.Parse(order.EndTime);
                // Get the current time
                // Convert the local time to IST
                DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, istTimeZone);


                // Compare the current time with the start time
                if ((currentTime.TimeOfDay < startTime || currentTime.TimeOfDay > endTime) && (order.OrderType == "ce_entry" || order.OrderType == "pe_entry"))
                {

                    return "Not in time window";
                }
                //if (order.OrderType.Contains("exit"))
                //{
                bool placeNewOrder = await Task.FromResult(true);


                var openOrders = nApi.SendGetPositionBook(responseHandler.OnResponse, order.UID);
                responseHandler.ResponseEvent.WaitOne();
                if (responseHandler.baseResponse != null)
                {
                    var bookResponse = responseHandler.baseResponse as PositionBookResponse;


                    if (bookResponse != null && bookResponse.stat != "Not_Ok")
                    {
                        var openPositions = bookResponse?.positions.Where(x => Convert.ToInt32(x.netqty) > 0);
                        foreach (var p in openPositions)
                        {


                            try
                            {

                                placeOrder = new PlaceOrder();
                                placeOrder.uid = order.UID;
                                placeOrder.actid = order.UID;
                                placeOrder.exch = order.Exchange;
                                placeOrder.tsym = p.tsym;

                                placeOrder.qty = p.netqty;
                                placeOrder.dscqty = "0";
                                placeOrder.prd = p.prd;

                                placeOrder.prc = "0";
                                placeOrder.prctyp = "MKT";
                                placeOrder.ret = "DAY";
                                placeOrder.ordersource = "API";


                                if (p.tsym.ToUpper().StartsWith(order.Asset.ToUpper()) && p.tsym.Substring(p.tsym.Length - 8).Contains("P") && (order.OrderType == "pe_exit" || order.OrderType == "ce_entry"))
                                {

                                    placeOrder.trantype = "S";

                                }
                                else if (p.tsym.ToUpper().StartsWith(order.Asset.ToUpper()) && p.tsym.Substring(p.tsym.Length - 8).Contains("C") && (order.OrderType == "ce_exit" || order.OrderType == "pe_entry"))
                                {


                                    placeOrder.trantype = "S";
                                }
                                else
                                {
                                    placeNewOrder = false;
                                }
                                if (placeOrder.trantype == "S")
                                {
                                    nApi.SendPlaceOrder(responseHandler.OnResponse, placeOrder);
                                    responseHandler.ResponseEvent.WaitOne();
                                    status = "Successfully placed exit order";
                                    //var sellResponse = responseHandler.baseResponse as PositionBookResponse;
                                    //Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());
                                }


                            }
                            catch (Exception ex)
                            {
                                //_logger.LogInformation("Error: " + ex.StackTrace);
                                status = loggedInUser + " :" + ex.StackTrace;

                            }


                        }
                    }
                }
                //}

                if (placeNewOrder && order.OrderType.Contains("entry"))
                {
                    placeOrder = new PlaceOrder();
                    placeOrder.uid = order.UID;
                    placeOrder.actid = order.UID;
                    placeOrder.exch = order.Exchange;
                    placeOrder.tsym = await helper.GetFOAsset(order);

                    placeOrder.qty = Convert.ToString(order.Quantity);
                    placeOrder.dscqty = "0";
                    placeOrder.prd = "M";
                    if (order.OrderType.Contains("entry"))
                    {
                        placeOrder.trantype = "B";
                    }

                    placeOrder.prc = "0";
                    placeOrder.prctyp = "MKT";
                    placeOrder.ret = "DAY";
                    placeOrder.ordersource = "API";
                    placeOrder.remarks = "";
                    if (order.OrderType.Contains("entry"))
                    {
                        nApi.SendPlaceOrder(responseHandler.OnResponse, placeOrder);
                        responseHandler.ResponseEvent.WaitOne();
                        status = "successfully placed buy order";
                        var orderResponse = responseHandler.baseResponse as PositionBookResponse;
                        status = status + " " + orderResponse.emsg;
                    }

                    //Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());
                    //return string.Empty;
                }


            }
            catch (Exception ex)
            {
                //_logger.LogInformation("Error: " + ex.StackTrace);
                return loggedInUser +ex.StackTrace + " Exits Orders: " + status;
            }
            return loggedInUser + ": " + status;
        }
    }
}
