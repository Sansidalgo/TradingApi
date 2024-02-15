using BLU.Enums;
using BLU.Repositories;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;


namespace sansidalgo.Server.Controllers.Trading
{
    [Route("api/[controller]")]
    [ApiController]
    public class NseController : ControllerBase
    {
        private readonly OptionsDataRepository oi;
        public NseController(OptionsDataRepository _oi)
        {
         
            oi = _oi;
        }

        [HttpGet]
        public async Task<DbStatus> GetOI()
        {
            var nseData = await oi.GetMarketData();

            return nseData;
        }
        [HttpGet("GetNSEData")]
        public async Task<DbStatus> GetNSEData()
        {
            DbStatus res = new DbStatus();
            res.Status = 0;
            try
            {
               
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    client.DefaultRequestHeaders.Referrer = new Uri("https://www.nseindia.com/"); // Set the referrer header if needed
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json"); // Set the accept header if needed

                    string url = "https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%2050";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                       var jsonContent= await response.Content.ReadAsStringAsync();

                        // Parse the entire JSON content
                        JObject jsonObj = JObject.Parse(jsonContent);

                        // Extract the "data" part
                        JArray data = jsonObj["data"] as JArray;

                        res.Result = data;
                        res.Status = 1;
                        
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        res.Message = $"error occured while reading data: {response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = 0;
            }
            return res;

        }


    }
    class OptionData
    {
        public string ExpiryDate { get; set; }
        public int StrikePrice { get; set; }
        public int OpenInterest { get; set; }
        public int OpenInterestChange { get; set; }
    }
}
