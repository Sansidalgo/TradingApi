using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using BLU.VendorLogics;
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

        private readonly Shoonya shoonya;
        private readonly CommonHelper helper;

        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        private readonly AlgoContext context;
        private readonly OrderSettingsRepository repo;
        private readonly ShoonyaCredentialsRepository shoonyaCredentialsRepo;
        private readonly IMapper mapper;
        public ShoonyaNewController(Shoonya _shoonya, CommonHelper _helper, AlgoContext _context, IMapper mapper)
        {
            context = _context;
            repo = new OrderSettingsRepository(_context);
            shoonyaCredentialsRepo = new ShoonyaCredentialsRepository(_context);
            this.mapper = mapper;
            shoonya = _shoonya;
            helper = _helper;

        }

        [HttpPost(Name = "ExecuteOrder")]
        public async Task<string> ExecuteOrder(ShoonyaOrder order)
        {
            OrderSettingsResponseDto orderSettings;


            orderSettings = (OrderSettingsResponseDto)(await this.repo.GetOrderSettingsById(CommonHelper.GetNumberFromString(order.OSID))).Result;

            DbStatus signInStatus = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
            if (signInStatus.Status == 1)
            {
                var status = await shoonya.ExecuteShoonyaOrder(orderSettings, order.IndexPrice, signInStatus.Result as ShoonyaReponseDto);
                logger.Info(status);
                return status.ToString();
            }
            else
            {
                logger.Info(signInStatus.Message);
                return signInStatus.Message;
            }


        }
    }
}
