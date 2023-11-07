using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors;
using sansidalgo.Models;

namespace sansidalgo.Controllers
{
    //Added this handle shoonya orders
    [Route("[controller]")]
    [ApiController]
    public class ShoonyaController : ControllerBase
    {


       
        private readonly ILogger<ShoonyaController> _logger;
        private readonly ShoonyaLogics shoonya;
        private readonly CommonHelper helper;

       private static NLog.Logger logger =LogManager.GetCurrentClassLogger();
        public ShoonyaController(ILogger<ShoonyaController> logger,ShoonyaLogics _shoonya,CommonHelper _helper)
        {
            _logger = logger;
            shoonya= _shoonya;
            helper=_helper;
         

        }
        [HttpGet("GetEncodedValue")]
        public async Task<string> GetEncodedValue(String value)
        {
           
            return await helper.EncodeValue(value);
        }

        [HttpGet("GetShoonya")]
        public IEnumerable<WeatherForecast> Get()
        {
            logger.Info("test");
           
            return shoonya.GetOrders();
        }
        [HttpPost(Name = "PostShoonya")]
        public async Task<string> PostShoonyaOrder(Order order)
        {
           var status= await shoonya.PostShoonyaOrder(order);
            logger.Info(status);
            return status.ToString();
        }
    }
}
