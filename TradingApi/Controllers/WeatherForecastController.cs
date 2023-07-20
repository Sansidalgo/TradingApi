using com.dakshata.autotrader.api;
using com.dakshata.constants.trading;
using com.dakshata.data.model.common;
using com.dakshata.trading.model.platform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace TradingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;
        private IAutoTrader autoTrader;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _clientId = "IUQK0HR3VQ-100";
            _clientSecret = "JSZ78ETIHP";
            _httpClient = new HttpClient();
           
        }
        [HttpPost(Name = "PostOrder")]
        public async Task PostOrder(Order order)
        {
            try
            {
                bool placeNewOrder = true;

                autoTrader = AutoTrader.CreateInstance(
        order.ApiKey,
        AutoTrader.SERVER_URL);
                IOperationResponse<ISet<PlatformPosition>> r12 = autoTrader.ReadPlatformPositions(order.peseudoAccount);
                Console.WriteLine("Message: {0}", r12.Message);

                foreach (PlatformPosition p in r12.Result.Where(w =>w.State==PositionState.OPEN && w.IndependentSymbol.ToLower().StartsWith(order?.Asset?.ToLower())))
                {
                    try
                    {
                        
                        

                        if (p.IndependentSymbol.ToLower().Contains("pe") && p.State == PositionState.OPEN && (order.OrderType == "pe_exit" || order.OrderType == "ce_entry"))
                        {

                            IOperationResponse<string> r7 = autoTrader.PlaceRegularOrder(order.peseudoAccount, order.Exchange, p.IndependentSymbol, TradeType.SELL,
                                OrderType.MARKET, ProductType.NORMAL, order.Quantity, 0f, 0);
                           
                        }
                        else if (p.IndependentSymbol.ToLower().Contains("ce") && p.State == PositionState.OPEN && ( order.OrderType == "ce_exit" || order.OrderType == "pe_entry"))
                        {

                            IOperationResponse<string> r7 = autoTrader.PlaceRegularOrder(order.peseudoAccount, order.Exchange, p.IndependentSymbol, TradeType.SELL,
                                OrderType.MARKET, ProductType.NORMAL, order.Quantity, 0f, 0);

                        }
                        
                      //if  (p.IndependentSymbol.ToLower().Contains("ce") && p.State == PositionState.OPEN && order.OrderType == "ce_entry")
                      //  {
                      //      placeNewOrder = false;
                      //      break;
                      //  }
                      //  else if (p.IndependentSymbol.ToLower().Contains("pe") && p.State == PositionState.OPEN && order.OrderType == "pe_entry")
                      //  {
                      //      placeNewOrder = false;
                      //      break;
                      //  }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Error: " + ex.Message);

                    }


                }
                r12 = autoTrader.ReadPlatformPositions(order.peseudoAccount);
                if (r12.Result.Where(w =>  w.State == PositionState.OPEN && w.IndependentSymbol.ToLower().StartsWith(order?.Asset?.ToLower())).Count()<=0 && (order.OrderType == "pe_entry" || order.OrderType == "ce_entry"))
                {


                    string asset = string.Empty;

                    if (order.Asset == "BANKNIFTY" && order.OrderType == "pe_entry")
                    {
                        asset = await GetStrikePrice(order.ExpiryDay,order.Asset, "PE", ((int)(order.IndexPrice / 100) * 100) - order.StrikePriceDifference);
                    }
                    else if (order.Asset == "BANKNIFTY" && order.OrderType == "ce_entry")
                    {
                        asset = await GetStrikePrice(order.ExpiryDay,order.Asset, "CE", ((int)(order.IndexPrice / 100) * 100) + order.StrikePriceDifference);
                    }
                    if (order.Asset == "NIFTY" && order.OrderType == "pe_entry")
                    {
                        asset = await GetStrikePrice(order.ExpiryDay,order.Asset, "PE", ((int)(order.IndexPrice / 100) * 100) - order.StrikePriceDifference);
                    }
                    else if (order.Asset == "NIFTY" && order.OrderType == "ce_entry")
                    {
                        asset = await GetStrikePrice(order.ExpiryDay,order.Asset, "CE", ((int)(order.IndexPrice / 100) * 100) + order.StrikePriceDifference);
                    }
                    else if (order.OrderType == "ce_entry")
                    {
                        asset = await GetStrikePrice(order.ExpiryDay,order.Asset, "CE", ((int)(order.IndexPrice / 100) * 100) + order.StrikePriceDifference);
                    }
                    else if (order.OrderType == "pe_entry")
                    {
                        asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "PE", ((int)(order.IndexPrice / 100) * 100) + order.StrikePriceDifference);
                    }

                    if (!string.IsNullOrWhiteSpace(asset))
                    {
                        IOperationResponse<string> r5 = autoTrader.PlaceRegularOrder(order.peseudoAccount, order.Exchange, asset, TradeType.BUY,
                            OrderType.MARKET, ProductType.NORMAL, order.Quantity, 0f, 0);
                        Console.WriteLine("Message: {0}", r5.Message);
                        Console.WriteLine("Result: {0}", r5.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: " + ex.Message);

            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<string> GetStrikePrice(string dayOfWeekString,string symbol, string optionType, decimal strikePrice)
        {

            optionType = optionType.ToUpper();
            DateTime currentDateTime = DateTime.Now;
           var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekString, true);

            // Calculate the expiry date for the current week (Thursday)
            DateTime expiryDate = currentDateTime.AddDays(dayOfWeek - currentDateTime.DayOfWeek);

            // Adjust expiry date if current day is after Thursday
            if (currentDateTime.DayOfWeek > dayOfWeek)
            {
                expiryDate = expiryDate.AddDays(7);
            }

            // Prepare the strike price symbol
            string strikePriceSymbol = symbol + "_" + $"{expiryDate:dd-MMM-yyyy}" + "_" + optionType + "_" + strikePrice;
            return await Task.FromResult(strikePriceSymbol.ToUpper());
        }
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            var kk = GetAuthorizationCode();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> GetAuthorizationCode()
        {
            string apiUrl = "https://api.fyers.in/api/v2/generate-authcode";
            var requestBody = new
            {
                appId = _clientId,
                redirectUrl = "https://srialgotrading.azurewebsites.net/Home/Index" // Use the out-of-band redirect URL
            };

            var response = await _httpClient.PostAsJsonAsync(apiUrl, requestBody);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Extract the authorization code from the response
            // The authorization code will be needed in the next step
            // Parse the response content and extract the code
            // Example: var authCode = ExtractCodeFromResponse(responseContent);

            return string.Empty;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> GetAccessToken(string authCode)
        {
            string apiUrl = "https://api.fyers.in/api/v3/generate-token";
            var requestBody = new
            {
                appId = _clientId,
                appSecret = _clientSecret,
                authorizationCode = authCode
            };

            var response = await _httpClient.PostAsJsonAsync(apiUrl, requestBody);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Extract the access token from the response
            // The access token can be used to make authenticated requests to the FYERS API
            // Parse the response content and extract the access token
            // Example: var accessToken = ExtractAccessTokenFromResponse(responseContent);

            return string.Empty;
        }
    }
}