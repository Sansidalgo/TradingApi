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
using System.Diagnostics;

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
            // Create a new Stopwatch instance for each user
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            OrderSettingsResponseDto orderSettings;
            DbStatus res = new DbStatus();
            logger.Info("Getting Order Settings details");

            try
            {
                res = await this.Seetingsrepo.GetOrderSettingsById(CommonHelper.GetNumberFromString(order.OSID));
                logger.Info(res.Message);
                orderSettings = (OrderSettingsResponseDto)res.Result;
                string user = string.Empty;

                if (!string.IsNullOrWhiteSpace(Convert.ToString(orderSettings.TraderId)))
                {
                    logger.Info("Order details received from db");
                    user = " User Id: " + CommonHelper.DecodeValue(orderSettings.Credential.Uid) + " Shoonya ID: " + orderSettings?.TraderId + " ";
                }
                else
                {
                    logger.Info("Order Setting does not exist");
                    return res;
                }

                res = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
                logger.Info(user + res.Message);
                ShoonyaReponseDto shoonyaResponse = res.Result as ShoonyaReponseDto;

                if (res.Status == 1)
                {
                    // Placeholder for the rest of your code
                    // Add your specific logic here, e.g., additional API calls, data processing, etc.

                    // Example: Perform some additional processing
                    string asset = await CommonHelper.GetFOAsset(orderSettings.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSettings.OptionsSetting.CeSideEntryAt), order.IndexPrice, orderSettings.OrderSide.Name, orderSettings.OptionsSetting.Instrument.ExpiryDay);
                    logger.Info(user + "asset: " + asset);

                    if (!(await orderRepo.CheckWhetherInTimeWindow(orderSettings.OptionsSetting.StartTime, orderSettings.OptionsSetting.StartTime)))
                    {
                        res.Message = "Not in time window";
                        logger.Info(user + res.Message);
                        res.Status = 0;
                        return res;
                    }

                    if (!orderSettings.Environment.Name.ToLower().Equals("papertrading"))
                    {
                        logger.Info(user + "Started Selling Live Order");
                        res = await orderRepo.PlaceSellOrderLive(orderSettings, shoonyaResponse);
                        logger.Info(user + res.Message);
                        logger.Info(user + "Completed Selling Live Order");

                        logger.Info(user + "Started Buying Live Order");
                        res = await orderRepo.PlaceBuyOrderLive(Convert.ToBoolean(res.Status), asset, orderSettings, order.IndexPrice, shoonyaResponse);
                        logger.Info(user + res.Message);
                        logger.Info(user + "Completed Buying Live Order");
                    }
                    else
                    {
                        logger.Info(user + "Started Selling Paper Order");
                        res = await orderRepo.PlacePaperSellOrder(orderSettings, shoonyaResponse);
                        logger.Info(user + res.Message);
                        logger.Info(user + "Completed Selling Paper Order");

                        logger.Info(user + "Started Buying Paper Order");
                        res = await orderRepo.PlacePaperBuyOrder(Convert.ToBoolean(res.Status), orderSettings, asset, order.IndexPrice, shoonyaResponse);
                        logger.Info(user + res.Message);
                        logger.Info(user + "Completed Buying Paper Order");
                    }

                    logger.Info("Shoonya User: " + CommonHelper.DecodeValue(orderSettings.Credential.Uid));
                    logger.Info(user + res.Message);

                    // End of the placeholder

                    stopwatch.Stop();
                    TimeSpan elapsedTime = stopwatch.Elapsed;
                    logger.Info($"API method execution time for {user}: {elapsedTime.TotalMilliseconds} milliseconds");

                    return res;
                }
                else
                {
                    logger.Info(user + res.Message);
                    return res;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                logger.Error($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception after logging or handling
            }
        }

    }
}
