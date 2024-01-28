using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly OrderRepository repo;
        public OrderController(AlgoContext _context)
        {
            context = _context;
            repo = new OrderRepository(_context);

        }
        [HttpGet("GetOrders")]
        public async Task<DbStatus> GetOrders()
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
            .Select(c => c.Value)
                .SingleOrDefault();


            return await this.repo.GetOrders(Convert.ToInt32(sid));
        }
    }
}
