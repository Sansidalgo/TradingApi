using AutoMapper;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors;
using sansidalgo.Server.Models;

namespace sansidalgo.Server.Controllers.Trading
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class ShoonyaNewController : ControllerBase
    {

        private readonly ShoonyaLogics shoonya;
        private readonly CommonHelper helper;

        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();
       
        private readonly AlgoContext context;
        private readonly OrderSettingsRepository repo;
        private readonly IMapper mapper;
        public ShoonyaNewController(ShoonyaLogics _shoonya, CommonHelper _helper,AlgoContext _context, IMapper mapper)
        {
            context = _context;
            repo = new OrderSettingsRepository(_context);
            this.mapper = mapper;
            shoonya = _shoonya;
            helper = _helper;

        }

        [HttpPost(Name = "PostShoonya")]
        public async Task<string> PostShoonyaOrder(ShoonyaOrder order)
        {
            TblOrderSetting orderSettings;
            if(!string.IsNullOrWhiteSpace(order.OrderSettingName))
            {
                orderSettings = (TblOrderSetting)(await this.repo.GetOrderSettingsByName(order.OrderSettingName)).Result;
            }
            else
            {

                orderSettings =(TblOrderSetting)(await this.repo.GetOrderSettingsById(order.OrderSettingId)).Result;
            }
       
            var status = await shoonya.ExecuteShoonyaOrder(orderSettings, order.IndexPrice);
            logger.Info(status);
            return status.ToString();
        }
    }
}
