using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sansidalgo.core.helpers;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {


        private readonly AlgoContext context;
        private readonly PlansRepository repo;

        public PlansController(AlgoContext _context, PlansRepository repo)
        {
            context = _context;
            this.repo = repo;
        }
        [HttpGet("GetPlans")]
        public async Task<DbStatus> GetPlans()
        {

            return await this.repo.GetPlans();
        }
    }
}
