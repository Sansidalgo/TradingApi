using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
       
        public ShoonyaController(ILogger<ShoonyaController> logger,ShoonyaLogics _shoonya)
        {
            _logger = logger;
            shoonya= _shoonya;

        }
      
        [HttpGet(Name = "GetShoonya")]
        public IEnumerable<WeatherForecast> Get()
        {
            return shoonya.GetOrders();
        }
        [HttpPost(Name = "PostShoonya")]
        public async Task<string> PostShoonyaOrder(Order order)
        {
           var status= await shoonya.PostShoonyaOrder(order);
            _logger.LogInformation(status);
            return status.ToString();
        }
    }
}
