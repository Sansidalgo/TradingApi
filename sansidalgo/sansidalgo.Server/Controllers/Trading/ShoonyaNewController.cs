using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors;
using sansidalgo.Server.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ShoonyaNewController : ControllerBase
{
    private readonly CommonHelper helper;

    private readonly AlgoContext context;
    private readonly OrderSettingsRepository Seetingsrepo;
    private readonly ShoonyaCredentialsRepository shoonyaCredentialsRepo;
    private readonly OrderRepository orderRepo;

    public ShoonyaNewController(CommonHelper _helper, AlgoContext _context, OrderSettingsRepository settingsRepo, ShoonyaCredentialsRepository credentialsRepo, OrderRepository repo)
    {
        context = _context;
        helper = _helper;

        Seetingsrepo = settingsRepo;
        shoonyaCredentialsRepo = credentialsRepo;
        orderRepo = repo;
    }
    [HttpPost("ExecuteOrderById")]
    public async Task<DbStatus> ExecuteOrderById(int orderSettingId)
    {
        var order = new ShoonyaOrder();
        order.OSID ="temp_"+ Convert.ToString(orderSettingId);


        return await ExecuteOrderLogic(order);
    }

    [HttpPost]
    public async Task<DbStatus> ExecuteOrder(ShoonyaOrder order)
    {

        return await ExecuteOrderLogic(order);
    }

    private async Task<DbStatus> ExecuteOrderLogic(ShoonyaOrder order)
    {
        var logger = LogManager.GetCurrentClassLogger();

        // Create a new Stopwatch instance for each user
        Stopwatch stopwatch = Stopwatch.StartNew();

        OrderSettingsResponseDto orderSettings;
        DbStatus res = new DbStatus();
        logger.Info("Getting Order Settings details");

        try
        {
            res = await Seetingsrepo.GetOrderSettingsById(CommonHelper.GetNumberFromString(order.OSID));
            logger.Info(res.Message);
            orderSettings = (OrderSettingsResponseDto)res.Result;
            string user = string.Empty;

            if (!string.IsNullOrWhiteSpace(Convert.ToString(orderSettings.TraderId)))
            {
                logger.Info("Order details received from db");
                user = $" User Id: {CommonHelper.DecodeValue(orderSettings.Credential.Uid)} Shoonya ID: {orderSettings?.TraderId} ";

            }
            else
            {
                logger.Info("Order Setting does not exist");
                return res;
            }

            res = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
            logger.Info($"{user}{res.Message}");
            ShoonyaReponseDto shoonyaResponse = res.Result as ShoonyaReponseDto;
            if (order.IndexPrice <= 0)
            {
                res = await orderRepo.GetIndex(shoonyaResponse, orderSettings.OptionsSetting.Instrument.Exchange, orderSettings.OptionsSetting.Instrument.Name);
                order.IndexPrice = Convert.ToDecimal(res.Result);

            }
            if (res.Status == 1)
            {
                // Placeholder for the rest of your code
                // Add your specific logic here, e.g., additional API calls, data processing, etc.

                // Example: Perform some additional processing
                string asset = await CommonHelper.GetFOAsset(orderSettings.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSettings.OptionsSetting.CeSideEntryAt), order.IndexPrice, orderSettings.OrderSide.Name, orderSettings.OptionsSetting.Instrument.ExpiryDay);
                logger.Info($"{user}asset: {asset}");

                if (!(await orderRepo.CheckWhetherInTimeWindow(orderSettings.OptionsSetting.StartTime, orderSettings.OptionsSetting.StartTime)))
                {
                    res.Message = "Not in time window";
                    logger.Info($"{user}{res.Message}");
                    res.Status = 0;
                    return res;
                }

                if (!orderSettings.Environment.Name.ToLower().Equals("papertrading"))
                {
                    logger.Info($"{user}Started Selling Live Order");
                    res = await orderRepo.PlaceSellOrderLive(orderSettings, shoonyaResponse);
                    logger.Info($"{user}{res.Message}");
                    logger.Info($"{user}Completed Selling Live Order");

                    logger.Info($"{user}Started Buying Live Order");
                    res = await orderRepo.PlaceBuyOrderLive(Convert.ToBoolean(res.Status), asset, orderSettings, order.IndexPrice, shoonyaResponse);
                    logger.Info($"{user}{res.Message}");
                    logger.Info($"{user}Completed Buying Live Order");
                }
                else
                {
                    logger.Info($"{user}Started Selling Paper Order");
                    res = await orderRepo.PlacePaperSellOrder(orderSettings, shoonyaResponse);
                    logger.Info($"{user}{res.Message}");
                    logger.Info($"{user}Completed Selling Paper Order");

                    logger.Info($"{user}Started Buying Paper Order");
                    res = await orderRepo.PlacePaperBuyOrder(Convert.ToBoolean(res.Status), orderSettings, asset, order.IndexPrice, shoonyaResponse);
                    logger.Info($"{user}{res.Message}");
                    logger.Info($"{user}Completed Buying Paper Order");
                }

                logger.Info($"Shoonya User: {CommonHelper.DecodeValue(orderSettings.Credential.Uid)}");
                logger.Info($"{user}{res.Message}");

                // End of the placeholder

                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;
                logger.Info($"API method execution time for {user}: {elapsedTime.TotalMilliseconds} milliseconds");

                return res;
            }
            else
            {
                logger.Info($"{user}{res.Message}");
                return res;
            }
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            logger.Info($"An error occurred: {ex.Message}");
            throw; // Re-throw the exception after logging or handling
        }
    }
}