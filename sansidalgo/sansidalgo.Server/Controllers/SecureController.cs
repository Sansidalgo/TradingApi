using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sansidalgo.core.helpers;

namespace sansidalgo.Server.Controllers
{
   
    [Authorize(Roles = "user")]
    [Route("[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecureData()
        {
            // This endpoint requires authentication
            var username = User.Identity.Name;
            return Ok($"Hello, {username}! This is secure data.");
        }
        [HttpGet("GetEncodedValue")]
        public string GetEncodedValue(String value)
        {

            return  CommonHelper.EncodeValue(value);
        }
        [HttpGet("GetDecodedValue")]
        public string GetDecodedValue(String value)
        {

            return CommonHelper.DecodeValue(value);
        }

    }
}
