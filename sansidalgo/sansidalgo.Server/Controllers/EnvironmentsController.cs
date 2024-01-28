using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentsController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly EnvironmentsRepository repo;
        public EnvironmentsController(AlgoContext _context)
        {
            context = _context;
            repo = new EnvironmentsRepository(_context);

        }
        [HttpGet("GetEnvironments")]
        public async Task<DbStatus> GetEnvironments()
        {

            return await this.repo.GetEnvironments();
        }
    }
}
