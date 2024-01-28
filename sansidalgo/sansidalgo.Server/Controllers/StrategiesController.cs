using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrategiesController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly StrategiesRepository repo;
        private readonly IMapper mapper;
        public StrategiesController(AlgoContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            repo = new StrategiesRepository(_context);


        }
        [HttpPost("Add")]
        public async Task<ActionResult<DbStatus>> Add(StrategyResponseDto strategy)
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value)
                .SingleOrDefault();

            strategy.TraderId = Convert.ToInt32(sid);
            strategy.CreatedBy = strategy.TraderId;
            strategy.CreatedDt = DateTime.UtcNow;
            return await this.repo.Add(strategy,mapper);


        }

        [HttpGet("GetStrategies")]
        public async Task<DbStatus> GetStrategies()
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
            .Select(c => c.Value)
                .SingleOrDefault();


            return await this.repo.GetStrategies(Convert.ToInt32(sid));
        }
    }
}
