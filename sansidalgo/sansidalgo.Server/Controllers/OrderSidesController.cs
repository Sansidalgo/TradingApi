using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSidesController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly OrderSidesRepository repo;
        public OrderSidesController(AlgoContext _context)
        {
            context = _context;
            repo = new OrderSidesRepository(_context);

        }
        [HttpGet("GetOrderSides")]
        public async Task<DbStatus> GetOrderSides()
        {

            return await this.repo.GetOrderSides();
        }
    }
}
