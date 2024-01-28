using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace sansidalgo.Server.Controllers.Trading
{
    [Route("api/[controller]")]
    [ApiController]
    public class NseController : ControllerBase
    {
        private readonly NseApiService _nseApiService;

        private readonly IHttpClientFactory _httpClientFactory;

        public NseController(IHttpClientFactory httpClientFactory, NseApiService nseApiService)
        {
            _httpClientFactory = httpClientFactory;
            _nseApiService = nseApiService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetNseData()
        {
            
            string endpoint = "get-quotes/derivatives?symbol=NIFTY";
            string nseData = await _nseApiService.GetNseDataAsync(endpoint);

            if (nseData != null)
                return Ok(nseData);
            else
                return BadRequest("Failed to retrieve NSE data.");
        }
        [HttpGet("GetPCRAndVWAP")]
        public async Task<IActionResult> GetPCRAndVWAP()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://www.nseindia.com/api/option-chain-indices?symbol=NIFTY");
               request.Headers.Add("Cookie", "ak_bmsc=BD4B1CC6237C4D2650F973B65F035FB1~000000000000000000000000000000~YAAQQ3xBF8t6pPWMAQAAvFriNhbO5ndi5T+qWodkOxvPKHy1zxy9Ko5SYwv6FVdKtsfeDypuUtPfPZPhieY/I9AH7GNT5JsfdHtgAV+milD4ML5+tLee+yVVvXKDfwoqskQb0nLU31JWTDBrjjeuNYcbD1BUnQW1d+S8NuSiesCyMHfo39jXqg1XdScWTmt+WlScjz+hugeskbIgIwyuPyKp2urfxb9x6P0wr1UX/qjHV8AtJRbrOr98LSUOb1iEdTQ+UMq6iZzCyVnmlWurwujmjW18qvCt+QUiab3RzaLe9mF9jVH4e89ZV6iZ/9/OpyKqMwkLkf5Rr5eFjEQYxqJeSF4PtNQI3Vm0zR3ffmrVWfTGtIdKuIU=; bm_sv=53813004B7E943DFA8F70F10730BC119~YAAQQ3xBF36lpPWMAQAA6O7kNhadymxrsk/LwSgM8540kYli2eGQ1u2NGjfgKlDMFSCQ/Eya46EFhyWmLCjTB7JEqifBd1s9coH+UjW4LUVMX2CdFrbbQxanZUJvt9X2JeRUFe7Mx1fGIkPTSsbCf9vxFOojxpQK5INjbz5vKjWQJO8Mg4Fm2g6tcInnHOlR2BCKUJVX2T3zwJTmi1xb0rrK9jItEfxQjOVMMApToQ4PjA/5JsKULrLl3imL9PVTzj4Q~1");
               
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());





                string endpoint = "option-chain-indices?symbol=NIFTY";
                string jsonData = await _nseApiService.GetNseDataAsync(endpoint);
                // Fetch data from NSE API
                //string jsonData = await GetDataFromApi(apiUrl);

                // Parse JSON data
                dynamic optionChainData = JObject.Parse(jsonData);

                // Calculate PCR and VWAP
                double pcr = CalculatePCR(optionChainData);
                //double vwap = CalculateVWAP(optionChainData);

                // Return results
                return Ok(new { PCR = pcr });
                //return Ok(new { PCR = pcr, VWAP = vwap });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        private async Task<string> GetDataFromApi(string apiUrl)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                // Set headers if needed
                client.DefaultRequestHeaders.Add("User-Agent", "YourUserAgent");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read and return the response content
                return await response.Content.ReadAsStringAsync();
            }
        }

        private double CalculatePCR(dynamic optionChainData)
        {
            // Implement your logic to calculate PCR here
            double totalPutOI = (double)optionChainData.records[0].CE.openInterest;
            double totalCallOI = (double)optionChainData.records[0].PE.openInterest;

            return totalPutOI / totalCallOI;
        }
        //private double CalculateVWAP(dynamic optionChainData)
        //{
        //    // Implement your logic to calculate VWAP here
        //    var prices = optionChainData.records[0].CE.data.Select(item => (double)item.CE);
        //    var volumes = optionChainData.records[0].CE.data.Select(item => (double)item.CEVolume);

        //    // Use a delegate to avoid the lambda expression issue
        //    Func<double, double, double> multiplyAndSum = (price, volume) => price * volume;

        //    return prices.Zip(volumes, new Func<double, double, double>(multiplyAndSum)).Sum() / volumes.Sum();
        //}

    }
}
