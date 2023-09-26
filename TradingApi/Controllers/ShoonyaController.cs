
using Microsoft.AspNetCore.Mvc;
using NorenRestApiWrapper;


namespace TradingApi.Controllers
{
    //Added this handle shoonya orders
    [Route("[controller]")]
    [ApiController]
    public class ShoonyaController : ControllerBase
    {


        public static NorenRestApi nApi;
        LoginMessage loginMessage;
        private readonly ILogger<ShoonyaController> _logger;
        private readonly BaseResponseHandler responseHandler;
        public ShoonyaController(ILogger<ShoonyaController> logger)
        {
            _logger = logger;


        }
        //added this to test
        private static readonly string[] Summaries = new[]
       {
        "FA155912", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        [HttpGet(Name = "GetShoonya")]
        public IEnumerable<WeatherForecast> Get()
        {



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost(Name = "PostShoonya")]
        public async Task<string> PostShoonyaOrder(Order order)
        {
            string loggedInUser = string.Empty;
            string status = string.Empty;
            bool OkayToPlaceOrder = true;
            PlaceOrder placeOrder;
            try
            {
                if(!Summaries.Any(w=>w.Equals(order.UID)))
                {
                    return "UID: "+order.UID+" is not registered";
                }
                nApi = new NorenRestApi();
                var endPoint = "https://api.shoonya.com/NorenWClientTP/";
                LoginMessage loginMessage = new LoginMessage();
                loginMessage.apkversion = "1.0.0";
                loginMessage.uid = order.UID;
                loginMessage.pwd = order.PSW;
                loginMessage.factor2 = CommonHelper.GetTOTP(order.authSecretekey);
                loginMessage.imei = order.imei;
                loginMessage.vc = order.VC;
                loginMessage.source = "API";
                loginMessage.appkey = order.ApiKey;
                var responseHandler = new BaseResponseHandler();

                nApi.SendLogin(responseHandler.OnResponse, endPoint, loginMessage);

                responseHandler.ResponseEvent.WaitOne();

                LoginResponse loginResponse = responseHandler.baseResponse as LoginResponse;
                Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());
                _logger.LogInformation("Logged in user:" + loginResponse.uname);
                loggedInUser="User:"+loginResponse.uname;

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
                bool placeNewOrder = await Task.FromResult(true);


                var openOrders = nApi.SendGetPositionBook(responseHandler.OnResponse, order.UID);
                responseHandler.ResponseEvent.WaitOne();
                if (responseHandler.baseResponse != null)
                {
                    var bookResponse = responseHandler.baseResponse as PositionBookResponse;
                   
                    if (bookResponse != null && bookResponse.stat!= "Not_Ok")
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
                                _logger.LogInformation("Error: " + ex.StackTrace);
                                status =loggedInUser+" :"+ ex.StackTrace;
                                OkayToPlaceOrder = false;
                            }


                        }
                    }
                }
                if (OkayToPlaceOrder && order.OrderType.Contains("entry"))
                {
                    placeOrder = new PlaceOrder();
                    placeOrder.uid = order.UID;
                    placeOrder.actid = order.UID;
                    placeOrder.exch = order.Exchange;
                    placeOrder.tsym = await CommonHelper.GetFOAsset(order);

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
                    if (order.OrderType.Contains("entry"))
                    {
                        nApi.SendPlaceOrder(responseHandler.OnResponse, placeOrder);
                        responseHandler.ResponseEvent.WaitOne();
                        status = "successfully placed buy order";
                        var orderResponse = responseHandler.baseResponse as PositionBookResponse;
                    }

                    //Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());
                    //return string.Empty;
                }


            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: " + ex.StackTrace) ;
                return loggedInUser + " :" + ex.StackTrace +" Exits Orders: "+status;
            } 
            return string.Empty;
        }
    }
}

