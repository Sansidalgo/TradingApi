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
       

    }
}
