using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly UserSubscriptionRepository repo;
        public SubscriptionsController(AlgoContext _context, UserSubscriptionRepository _repo)
        {
            context = _context;
            repo = _repo;

        }
        [HttpGet("GetSubscriptions")]
        public async Task<DbStatus> GetSubscriptions()
        {

            return await this.repo.GetSubscriptions();
        }

    }
}
