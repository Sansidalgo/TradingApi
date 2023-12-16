using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    //[Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSettingsController : ControllerBase
    {

        private readonly AlgoContext context;
        private readonly OrderSettingsRepository repo;
        private readonly IMapper mapper;
        public OrderSettingsController(AlgoContext _context, IMapper mapper)
        {
            context = _context;
            repo = new OrderSettingsRepository(_context);
            this.mapper = mapper;

        }
        [HttpPost("Add")]
        public async Task<ActionResult<DbStatus>> Add(OrderSettingsRequestDto settings)
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value)
                .SingleOrDefault();

            settings.TraderId = Convert.ToInt32(sid);
            //settings.CreatedBy = settings.TraderId;
            //settings.CreatedDt = DateTime.UtcNow;
            return await this.repo.Add(settings,this.mapper);


        }

        [HttpGet("GetOrderSettings")]
        public async Task<DbStatus> GetOrderSettings()
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
            .Select(c => c.Value)
                .SingleOrDefault();


            return await this.repo.GetOrderSettings(Convert.ToInt32(sid));
        }

        [HttpGet("GetOrderSettingsById")]
        public async Task<DbStatus> GetOrderSettingsById(int orderSettingId)
        {
           

            return await this.repo.GetOrderSettingsById(Convert.ToInt32(orderSettingId));
        }
    }
}
