using BLU.Enums;
using BLU.Repositories;

using Microsoft.AspNetCore.Mvc;


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
    //    [HttpGet("GetNSEData")]
    //    public async Task<string> GetNSEData()
    //    {
    //        try
    //        {
    //            using (HttpClient client = new HttpClient())
    //            {
    //                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
    //                client.DefaultRequestHeaders.Referrer = new Uri("https://www.nseindia.com/"); // Set the referrer header if needed
    //                client.DefaultRequestHeaders.Accept.ParseAdd("application/json"); // Set the accept header if needed

    //                string url = "https://www.nseindia.com/api/option-chain-indices?symbol=NIFTY";

    //                HttpResponseMessage response = await client.GetAsync(url);

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    string apiData = await response.Content.ReadAsStringAsync();
    //                    // Deserialize API data
    //                    var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(apiData);
    //                    var currentWeekOptionData = ((IEnumerable<dynamic>)apiResponse.records.data)
    //.Where((Func<dynamic, bool>)(option => Convert.ToDateTime(option.expiryDate) == apiResponse.records.expiryDates.First()))
    //.ToList();

    //                    var topSupportLevel = ((Func<IEnumerable<dynamic>, dynamic>)(data => data
    //.OrderByDescending(option => option.OpenInterest)
    //.FirstOrDefault()))(currentWeekOptionData);


    //                    var topResistanceLevel = ((Func<IEnumerable<dynamic>, dynamic>)(data => data
    // .OrderByDescending(option => option.OpenInterestChange)
    // .FirstOrDefault()))(currentWeekOptionData);


    //                    // Output the results
    //                    Console.WriteLine($"Support Level: Expiry Date: {topSupportLevel.ExpiryDate}, Strike Price: {topSupportLevel.StrikePrice}, OI: {topSupportLevel.OpenInterest}");
    //                    Console.WriteLine($"Resistance Level: Expiry Date: {topResistanceLevel.ExpiryDate}, Strike Price: {topResistanceLevel.StrikePrice}, OI Change: {topResistanceLevel.OpenInterestChange}");
    //                    return apiData;
    //                }
    //                else
    //                {
    //                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    //                    return "";
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }


    //    }


    }
    class OptionData
    {
        public string ExpiryDate { get; set; }
        public int StrikePrice { get; set; }
        public int OpenInterest { get; set; }
        public int OpenInterestChange { get; set; }
    }
}
