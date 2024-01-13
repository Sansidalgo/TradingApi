using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using sansidalgo.core.helpers;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSettingsController : ControllerBase
    {

        private readonly AlgoContext context;
        private readonly OrderSettingsRepository repo;
        private readonly IMapper mapper;
        Logger logger = LogManager.GetCurrentClassLogger();
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

        [HttpGet("GetOrderSettingsByIdForApi")]
        public async Task<DbStatus> GetOrderSettingsByIdForApi(int orderSettingId)
        {
            try
            {
                var res = await this.repo.GetOrderSettingsByIdForApi(Convert.ToInt32(orderSettingId));

                return res;
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                throw ex;
            }
          
        }
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<DbStatus> Delete(int id)
        {
            return await this.repo.Delete(id);
        }
    }
}
