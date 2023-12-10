using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
