using DataLayer.Models;
using NLog;
using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using sansidalgo.core.Helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors.Interfaces;
using System.Text;
using BLU;
using BLU.Dtos;
using BLU.VendorLogics.Interfaces;
namespace BLU.VendorLogics
{
    public class Shoonya : IShoonya
    {
        public static NorenRestApi? nApi;
        LoginMessage? loginMessage;
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private readonly BaseResponseHandler? responseHandler;
       
        private readonly CommonHelper helper;
        public Shoonya(CommonHelper _helper)
        {
            helper = _helper;
            
        }

        public async Task<string> ExecuteShoonyaOrder(OrderSettingsResponseDto order,decimal IndexPrice)
        {

           
            string loggedInUser = string.Empty;
            StringBuilder status = new StringBuilder();

            PlaceOrder placeOrder;
            try
            {

                //if (!Summaries.Any(w => w.Equals(order.Credential.Uid)))
                //{
                //    return await Task.FromResult(string.Concat("UID: ", order.Credential.Uid, " is not registered"));
                //}
                nApi = new NorenRestApi();
                var endPoint = "https://api.shoonya.com/NorenWClientTP/";
                LoginMessage loginMessage = new LoginMessage();
                loginMessage.apkversion = "1.0.0";
                loginMessage.uid = order.Credential.Uid.Trim();
                loginMessage.pwd = order.Credential.Password.Trim();
                loginMessage.vc = order.Credential.Vc.Trim();
                loginMessage.appkey = order.Credential.ApiKey.Trim();
                loginMessage.imei = order.Credential.Imei.Trim();
                loginMessage.source = "API";
                logger.Info("getting OTP Started");
                int cntr = 0;
            retryOtp:
                OtpEntity oe = await helper.GetTOTP(order.Credential.AuthSecreteKey.Trim());
                while (oe.RemaingTime < 2)
                {
                    oe = await helper.GetTOTP(order.Credential.AuthSecreteKey);
                }
                loginMessage.factor2 = oe.OTP;

                var responseHandler = new BaseResponseHandler();

                nApi.SendLogin(responseHandler.OnResponse, endPoint, loginMessage);

                await Task.FromResult(responseHandler.ResponseEvent.WaitOne());

                LoginResponse? loginResponse = responseHandler?.baseResponse as LoginResponse;
                order.Token = loginResponse.susertoken;

                nApi.SetSession(endPoint, loginMessage.uid, loginMessage.pwd,order.Token);
                
                Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());
                if (cntr < 2 && (loginResponse?.emsg != null && Convert.ToString(loginResponse?.emsg).ToLower().Contains("session expired")))
                {
                    cntr++;
                    await Task.Delay(1000);
                    goto retryOtp;
                }
                if (loginResponse?.emsg != null)
                {
                    status.Append(loginResponse?.emsg);
                    status.Append('\n');


                    logger.Info(responseHandler.baseResponse.toJson());
                }

                //_logger.LogInformation("Logged in user:" + loginResponse.uname);
                loggedInUser = "User:" + loginResponse?.uname;

                // Create a TimeZoneInfo object for Indian Standard Time (IST)
                TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                TimeSpan startTime = TimeSpan.Parse(order.OptionsSetting.StartTime);
                TimeSpan endTime = TimeSpan.Parse(order.OptionsSetting.EndTime);
                // Get the current time
                // Convert the local time to IST
                DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, istTimeZone);


                // Compare the current time with the start time
                if ((currentTime.TimeOfDay < startTime || currentTime.TimeOfDay > endTime) && (order.OrderSide.Name == "cebuy" || order.OrderSide.Name == "cebuy"))
                {

                    return "Not in time window";
                }
                //if (order.OrderType.Contains("exit"))
                //{
                bool placeNewOrder = await Task.FromResult(true);


                var openOrders = nApi.SendGetPositionBook(responseHandler.OnResponse, order.Credential.Uid);
                await Task.FromResult(responseHandler.ResponseEvent.WaitOne());
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
                                placeOrder.uid = order.Credential.Uid;
                                placeOrder.actid = order.Credential.Uid;
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
                                    placeNewOrder = false;
                                }
                                if (placeOrder.trantype == "S")
                                {
                                    nApi.SendPlaceOrder(responseHandler.OnResponse, placeOrder);
                                    await Task.FromResult(responseHandler.ResponseEvent.WaitOne());

                                    status.Append("Successfully placed exit order" + responseHandler?.baseResponse?.toJson());
                                    status.Append('\n');
                                    placeNewOrder = true;

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
                }
                //}

                if (placeNewOrder && order.OrderSide.Name.Contains("buy"))
                {
                    placeOrder = new PlaceOrder();
                    placeOrder.uid = order.Credential.Uid;
                    placeOrder.actid = order.Credential.Uid;
                    placeOrder.exch = order.OptionsSetting.Exchange;
                    placeOrder.tsym = await helper.GetFOAsset(order.OptionsSetting.Instrument.Name, Convert.ToInt32(order.OptionsSetting.CeSideEntryAt), IndexPrice, order.OrderSide.Name, order.OptionsSetting.Instrument.ExpiryDay);
                    logger.Info("Prepared Asset: " + Convert.ToString(placeOrder.tsym));
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
                        nApi.SendPlaceOrder(responseHandler.OnResponse, placeOrder);
                        await Task.FromResult(responseHandler.ResponseEvent.WaitOne());

                        var orderResponse = responseHandler.baseResponse as PlaceOrderResponse;
                        status.Append("successfully placed buy order");
                        status.Append('\n');
                        status.Append(orderResponse?.toJson());
                        status.Append('\n');
                        placeNewOrder = true;
                    }

                    Console.WriteLine("app handler :" + responseHandler?.baseResponse?.toJson());
                    //return string.Empty;
                }


            }
            catch (Exception ex)
            {
                //_logger.LogInformation("Error: " + ex.StackTrace);
                return loggedInUser + ex.StackTrace + " Exits Orders: " + status;
            }
            return loggedInUser + ": " + status.ToString();
        }
    }
}
