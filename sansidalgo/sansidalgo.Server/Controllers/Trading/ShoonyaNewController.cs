using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
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


        private readonly CommonHelper helper;

        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        private readonly AlgoContext context;
        private readonly OrderSettingsRepository Seetingsrepo;
        private readonly ShoonyaCredentialsRepository shoonyaCredentialsRepo;
        private readonly OrderRepository orderRepo;
    
        public ShoonyaNewController(CommonHelper _helper, AlgoContext _context)
        {
            context = _context;
            Seetingsrepo = new OrderSettingsRepository(_context);          
            shoonyaCredentialsRepo = new ShoonyaCredentialsRepository(_context);
            orderRepo = new OrderRepository(_context);

            helper = _helper;

        }

        [HttpPost(Name = "ExecuteOrder")]
        public async Task<DbStatus> ExecuteOrder(ShoonyaOrder order)
        {
            OrderSettingsResponseDto orderSettings;
            DbStatus res = new DbStatus();

            

            orderSettings = (OrderSettingsResponseDto)(await this.Seetingsrepo.GetOrderSettingsById(CommonHelper.GetNumberFromString(order.OSID))).Result;
            if(!string.IsNullOrWhiteSpace(Convert.ToString(orderSettings.TraderId)))
            {
                logger.Info("Trade Synergies User: " + orderSettings?.TraderId);
            }
            else
            {
                logger.Info("Order Setting does not exists");

            }
          

            res = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
            logger.Info(res.Message);
            ShoonyaReponseDto shoonyaResponse = res.Result as ShoonyaReponseDto;

            if (res.Status == 1)
            {
                //res=await orderRepo.GetIndex(shoonyaResponse, orderSettings.OptionsSetting.Exchange, orderSettings.OptionsSetting.Instrument.Name);

                string asset = await CommonHelper.GetFOAsset(orderSettings.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSettings.OptionsSetting.CeSideEntryAt), order.IndexPrice, orderSettings.OrderSide.Name, orderSettings.OptionsSetting.Instrument.ExpiryDay);

                if (!(await orderRepo.CheckWhetherInTimeWindow(orderSettings.OptionsSetting.StartTime, orderSettings.OptionsSetting.StartTime)))
                {
                    res.Message = "Not in time window";
                    res.Status = 0;
                    return res;
                }

                if (!orderSettings.Environment.Name.ToLower().Equals("papertrading"))
                {
                    res = await orderRepo.PlaceSellOrderLive(orderSettings, shoonyaResponse);
                    res = await orderRepo.PlaceBuyOrderLive(Convert.ToBoolean(res.Status), asset, orderSettings, order.IndexPrice, shoonyaResponse);

                }
                else
                {
                    res=await orderRepo.PlacePaperSellOrder(orderSettings, shoonyaResponse);
                    res = await orderRepo.PlacePaperBuyOrder(Convert.ToBoolean(res.Status), orderSettings,asset, order.IndexPrice, shoonyaResponse);
                }

                logger.Info("Shoonya User: " + CommonHelper.DecodeValue(orderSettings.Credential.Uid));
                logger.Info(res.Message);
                
                return res;
            }
            else
            {
                logger.Info(res.Message);
                return res;
            }

            

        }
    }
}
