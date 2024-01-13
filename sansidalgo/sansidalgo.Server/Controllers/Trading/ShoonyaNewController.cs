using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
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
    private readonly OrderSettingsRepository setingsRepo;
    private readonly ShoonyaCredentialsRepository shoonyaCredentialsRepo;
    private readonly OrderRepository orderRepo;

    public ShoonyaNewController(CommonHelper _helper, AlgoContext _context, OrderSettingsRepository settingsRepo, ShoonyaCredentialsRepository credentialsRepo, OrderRepository repo)
    {
        context = _context;
        helper = _helper;

        setingsRepo = settingsRepo;
        shoonyaCredentialsRepo = credentialsRepo;
        orderRepo = repo;
    }

    [HttpPost("ExecuteOrderById")]
    public async Task<DbStatus> ExecuteOrderById(int orderSettingId)
    {
        var order = new ShoonyaOrder();
        order.OSID = "temp_" + Convert.ToString(orderSettingId);

        return await orderRepo.ExecuteOrderLogic(order, setingsRepo, shoonyaCredentialsRepo);
    }

    [HttpPost]
    public async Task<DbStatus> ExecuteOrder(ShoonyaOrder order)
    {
        return await ExecuteOrderLogic(order);
    }

    private async Task<DbStatus> ExecuteOrderLogic(ShoonyaOrder order)
    {
        var logger =await Task.Run(()=> LogManager.GetCurrentClassLogger());
        Stopwatch stopwatch =await Task.Run(() => Stopwatch.StartNew());
        OrderSettingsResponseDto orderSettings;
        DbStatus res = await Task.Run(() => new DbStatus());
        await CommonHelper.InfoAsync("Getting Order Settings details", logger);

        try
        {
            res = await setingsRepo.GetOrderSettingsById(await CommonHelper.GetNumberFromString(order.OSID));
            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(order));
            await CommonHelper.InfoAsync($"Input :{jsonString}", logger);
            if (res.Result == null || res.Message == null)
            {
                await CommonHelper.InfoAsync("Order Setting does not exist", logger );
                await CommonHelper.InfoAsync($"Order sid:  {order.OSID}", logger);
                return res;
            }
            await CommonHelper.InfoAsync(res.Message, logger);

            orderSettings = (OrderSettingsResponseDto)res.Result;
            string user = string.Empty;

            if (!string.IsNullOrWhiteSpace(Convert.ToString(orderSettings.TraderId)))
            {
                await CommonHelper.InfoAsync("Order details received from db", logger);
                user = $" User Id: {await CommonHelper.DecodeValueAsync(orderSettings.Credential.Uid)} Shoonya ID: {orderSettings?.TraderId} ";
            }
            else
            {
                await CommonHelper.InfoAsync("Order Setting does not exist", logger);
                return res;
            }

            res = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
            await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
            if (res.Result == null)
            {
                await CommonHelper.InfoAsync("Logging failed", logger);
                return res;
            }
            ShoonyaReponseDto shoonyaResponse = res.Result as ShoonyaReponseDto;
            if (order.IndexPrice <= 0)
            {
                res = await orderRepo.GetIndex(shoonyaResponse, orderSettings.OptionsSetting.Instrument.Exchange, orderSettings.OptionsSetting.Instrument.Name);
                order.IndexPrice = Convert.ToDecimal(res.Result);
            }

            if (res.Status == 1)
            {
                string asset = await CommonHelper.GetFOAsset(orderSettings.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSettings.OptionsSetting.CeSideEntryAt), order.IndexPrice, orderSettings.OrderSide.Name, orderSettings.OptionsSetting.Instrument.ExpiryDay);
                await CommonHelper.InfoAsync($"{user}asset: {asset}", logger);

                if (!(await orderRepo.CheckWhetherInTimeWindow(orderSettings.OptionsSetting.StartTime, orderSettings.OptionsSetting.StartTime)))
                {
                    res.Message = "Not in time window";
                    await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                    res.Status = 0;
                    return res;
                }

                if (!orderSettings.Environment.Name.ToLower().Equals("papertrading"))
                {
                    await CommonHelper.InfoAsync($"{user}Started Selling Live Order", logger);
                    res = await orderRepo.PlaceSellOrderLive(orderSettings, shoonyaResponse);
                    await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                    await CommonHelper.InfoAsync($"{user}Completed Selling Live Order", logger);

                    await CommonHelper.InfoAsync($"{user}Started Buying Live Order", logger);
                    res = await orderRepo.PlaceBuyOrderLive(Convert.ToBoolean(res.Status), asset, orderSettings, order.IndexPrice, shoonyaResponse);
                    await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                    await CommonHelper.InfoAsync($"{user}Completed Buying Live Order", logger);
                }
                else
                {
                    await CommonHelper.InfoAsync($"{user}Started Selling Paper Order", logger);
                    res = await orderRepo.PlacePaperSellOrder(orderSettings, shoonyaResponse);
                    await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                    await CommonHelper.InfoAsync($"{user}Completed Selling Paper Order", logger);

                    await CommonHelper.InfoAsync($"{user}Started Buying Paper Order", logger);
                    res = await orderRepo.PlacePaperBuyOrder(Convert.ToBoolean(res.Status), orderSettings, asset, order.IndexPrice, shoonyaResponse);
                    await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                    await CommonHelper.InfoAsync($"{user}Completed Buying Paper Order", logger);
                }

                await CommonHelper.InfoAsync($"Shoonya User: {CommonHelper.DecodeValue(orderSettings.Credential.Uid)}", logger);
                await CommonHelper.InfoAsync($"{user}{res.Message}", logger     );

              await Task.Run(() => stopwatch.Stop());
                TimeSpan elapsedTime = stopwatch.Elapsed;
                await CommonHelper.InfoAsync($"API method execution time for {user}: {elapsedTime.TotalMilliseconds} milliseconds", logger);

                return res;
            }
            else
            {
                await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                return res;
            }
        }
        catch (Exception ex)
        {
             await CommonHelper.LogExceptionAsync(ex, logger);
            await CommonHelper.InfoAsync($"An error occurred: {ex.Message}",logger);
            throw;
        }
    }
}
