using BLU.Enums;
using BLU.Repositories;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class BrokersController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly BrokersRepository repo;
        public BrokersController(AlgoContext _context)
        {
            context = _context;
            repo = new BrokersRepository(_context);

        }
        [HttpGet("GetBrokers")]
        public async Task<DbStatus> GetBrokers()
        {
          
            return await this.repo.GetBrokers();
        }
    }
}
