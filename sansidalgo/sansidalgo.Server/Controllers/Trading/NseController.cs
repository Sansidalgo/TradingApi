using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sansidalgo.Server.Controllers.Trading
{
    [Route("api/[controller]")]
    [ApiController]
    public class NseController : ControllerBase
    {
        private readonly NseApiService _nseApiService;

        public NseController(NseApiService nseApiService)
        {
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
    }
}
