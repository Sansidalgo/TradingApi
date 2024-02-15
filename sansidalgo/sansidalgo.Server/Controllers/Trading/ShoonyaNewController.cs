using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors;
using sansidalgo.Server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

[Route("api/[controller]")]
[ApiController]
public class ShoonyaNewController : ControllerBase
{
    private readonly CommonHelper helper;
    private readonly AlgoContext context;
    private readonly OrderSettingsRepository setingsRepo;
    private readonly ShoonyaCredentialsRepository shoonyaCredentialsRepo;
    private readonly OrderRepository orderRepo;
    private readonly DelegateRepository delegateRepo;


    public ShoonyaNewController(CommonHelper _helper, AlgoContext _context, OrderSettingsRepository settingsRepo, ShoonyaCredentialsRepository credentialsRepo, OrderRepository repo, DelegateRepository _delegateRepo)
    {
        context = _context;
        helper = _helper;

        setingsRepo = settingsRepo;
        shoonyaCredentialsRepo = credentialsRepo;
        orderRepo = repo;
        delegateRepo = _delegateRepo;

    }

    [HttpPost("ExecuteOrderById")]
    public async Task<DbStatus> ExecuteOrderById(int orderSettingId)
    {
        var order = new ShoonyaOrder();
        order.OSID = "temp_" + Convert.ToString(orderSettingId);

        return await ExecuteBulkOrderLogic(order);
    }
    [NonAction]
    public string AddSpaceIfNeeded(string word)
    {
        return (word.ToLower().StartsWith("ce") || word.ToLower().StartsWith("pe")) ? " " + word : word;
    }
    [NonAction]
    public async Task<string> SendNotificationsTlgram(string messageText)
    {
        var result = string.Empty;
        try
        {
            string botToken = "6856503157:AAFDNZ9rPUZsi3A3YI89i0NtLpUZwAYQSi4";
            long chatId = 1001999680094;

            var botClient = new TelegramBotClient(botToken);
            //ChatId chatId = new ChatId(1001999680094);

            var chat = await botClient.GetChatAsync(new ChatId("@tradesynergies"));

         
            await botClient.SendTextMessageAsync(chat.Id, messageText);

            Console.WriteLine("SMS sent to Telegram!");
        }
        catch (Exception ex)
        {

            result = ex.Message;
        }
        return result;
    }
    [NonAction]
    public async Task<int> GetStrikePrice(string asset)
    {

        int strikePrice = 0;
        // Use regular expression to extract the number after the last character
        string pattern = @"(\d+)$";
        Match match = Regex.Match(asset, pattern);

        if (match.Success)
        {
            string extractedNumber = match.Groups[1].Value;
            strikePrice =await Task.FromResult(int.Parse(extractedNumber));

            
        }
        else
        {
            Console.WriteLine("No number found after the last character.");
        }
        return strikePrice;
    }

    [HttpPost]
    public async Task<DbStatus> ExecuteOrder(ShoonyaOrder order)
    {
       
        return await ExecuteBulkOrderLogic(order);
    }
    private async Task<DbStatus> ExecuteBulkOrderLogic(ShoonyaOrder order)
    {
        var logger = await Task.Run(() => LogManager.GetCurrentClassLogger());
        List<OrderSettingsResponseDto> orderSettings = new List<OrderSettingsResponseDto>();

        DbStatus res = await Task.Run(() => new DbStatus());
        try
        {
            res = await setingsRepo.GetOrderSettingsById(await CommonHelper.GetNumberFromString(order.OSID));

            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(order));
            await CommonHelper.InfoAsync($"Input :{jsonString}", logger);
            if (res.Result == null || res.Status == 0)
            {
                await CommonHelper.InfoAsync($"Order Setting Error: {res.Message}", logger);
                await CommonHelper.InfoAsync($"Order sid:  {order.OSID}", logger);
                return res;
            }
            await CommonHelper.InfoAsync(res.Message, logger);
            var orderSetting = (OrderSettingsResponseDto)res.Result;
            string asset = string.Empty;
            if (order.IndexPrice > 0)
            {

                asset = await CommonHelper.GetFOAsset(orderSetting.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSetting.OptionsSetting.CeSideEntryAt), order.IndexPrice, orderSetting.OrderSide.Name, orderSetting.OptionsSetting.Instrument.ExpiryDay);
                if (orderSetting.Trader.Id == 16)
                {
                    await SendNotificationsTlgram($"{AddSpaceIfNeeded(orderSetting.OrderSide.Name)} {orderSetting.OptionsSetting.Instrument.Name} option contract {await GetStrikePrice(asset)} At Current Price".ToUpper());
                }

            }
            orderSettings.Add(orderSetting);
            res = await setingsRepo.GetMasterTraderIdandOrderSideById(await CommonHelper.GetNumberFromString(order.OSID));
            DelegateReponseDto delegateReponseDto = (DelegateReponseDto)res.Result;
            res = await delegateRepo.GetDelegateByMasterId(delegateReponseDto.TraderId);
            if (res.Result != null)
            {
                try
                {
                    List<int?> tradeIds = (List<int?>)res.Result;

                    res = await setingsRepo.GetOrderSettingsByMasterId(tradeIds, delegateReponseDto.OrderSideId);
                    if (res.Result != null)
                    {
                        orderSettings.AddRange((List<OrderSettingsResponseDto>)res.Result);


                    }
                }
                catch (Exception ex)
                {
                    await CommonHelper.InfoAsync($"{ex.Message}", logger);

                }


            }

            foreach (var item in orderSettings)
            {
                try
                {
                    var res1 = await ExecuteOrderLogicNew(item, order.IndexPrice, asset);
                    res.Message = $"{res.Message} Final Status for User Id: {item.Trader.Id} {res1.Message} {Environment.NewLine}";
                    ;
                }
                catch (Exception ex)
                {
                    res.Message = $"{res.Message} Final Status for User Id: {item.Trader.Id} {ex.Message}  {Environment.NewLine}";
                    await CommonHelper.InfoAsync($"Error At ExecuteBulkOrderLogic: {ex.Message}", logger);
                }


            }
        }
        catch (Exception ex)
        {

            res.Message = ex.Message;
            await CommonHelper.InfoAsync($"{ex.Message}", logger);
        }
        return res;
    }
    private async Task<DbStatus> ExecuteOrderLogicNew(OrderSettingsResponseDto orderSettings, decimal IndexPrice,string asset)
    {
        var logger = await Task.Run(() => LogManager.GetCurrentClassLogger());
        Stopwatch stopwatch = await Task.Run(() => Stopwatch.StartNew());

        DbStatus res = await Task.Run(() => new DbStatus());
        await CommonHelper.InfoAsync("Getting Order Settings details", logger);

        try
        {

            string user = string.Empty;

            if (!string.IsNullOrWhiteSpace(Convert.ToString(orderSettings.TraderId)))
            {
                await CommonHelper.InfoAsync($"User Id: {orderSettings.TraderId}: Order details received from db", logger);
                user = $" User Id: {await CommonHelper.DecodeValueAsync(orderSettings.Credential.Uid)} Shoonya ID: {orderSettings?.TraderId} ";
            }
            else
            {
                await CommonHelper.InfoAsync("Order Setting does not exist", logger);
                return res;
            }

            res = await shoonyaCredentialsRepo.ShoonyaSignIn(orderSettings);
            await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
            if (res.Result == null || res.Status == 0)
            {
                await CommonHelper.InfoAsync("Logging failed", logger);
                return res;
            }
            ShoonyaReponseDto shoonyaResponse = res.Result as ShoonyaReponseDto;



            if (IndexPrice <= 0 && string.IsNullOrWhiteSpace(asset))
            {
                res = await orderRepo.GetIndex(shoonyaResponse, orderSettings.OptionsSetting.Instrument.Exchange, orderSettings.OptionsSetting.Instrument.Name);
                IndexPrice = Convert.ToDecimal(res.Result);
                asset = await CommonHelper.GetFOAsset(orderSettings.OptionsSetting.Instrument.Name, Convert.ToInt32(orderSettings.OptionsSetting.CeSideEntryAt), IndexPrice, orderSettings.OrderSide.Name, orderSettings.OptionsSetting.Instrument.ExpiryDay);

            }

            if (res.Status == 1)
            {
                
                //if (shoonyaResponse != null)
                //{
                //    await orderRepo.GetOptionChainAsync(shoonyaResponse, "NFO", asset, IndexPrice.ToString(), 5, orderSettings.OrderSide.Name);
                //}
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

                    res = await orderRepo.PlaceSellOrderLive(orderSettings, shoonyaResponse);

                    if (res.Status == 1)
                    {
                        res = await orderRepo.PlaceBuyOrderLive(Convert.ToBoolean(res.Status), asset, orderSettings, IndexPrice, shoonyaResponse);

                    }


                }
                else
                {

                    res = await orderRepo.PlacePaperSellOrder(orderSettings, shoonyaResponse);


                    res = await orderRepo.PlacePaperBuyOrder(Convert.ToBoolean(res.Status), orderSettings, asset, IndexPrice, shoonyaResponse);

                }

                await CommonHelper.InfoAsync($"Shoonya User: {CommonHelper.DecodeValue(orderSettings.Credential.Uid)}", logger);
                await CommonHelper.InfoAsync($"{user}{res.Message}", logger);

                await Task.Run(() => stopwatch.Stop());
                TimeSpan elapsedTime = stopwatch.Elapsed;
                await CommonHelper.InfoAsync($"API method execution time for {user}: {elapsedTime.TotalMilliseconds} milliseconds", logger);
                if (string.IsNullOrWhiteSpace(res.Message))
                {
                    res.Message = "Order Placed Successfully";
                }
                return res;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(res.Message))
                {
                    res.Message = "Error while getting index Successfully";
                }
                await CommonHelper.InfoAsync($"{user}{res.Message}", logger);
                return res;
            }
        }
        catch (Exception ex)
        {
            if (string.IsNullOrWhiteSpace(res.Message))
            {
                res.Message = ex.Message;
            }
            res.Status = 0;
            await CommonHelper.LogExceptionAsync(ex, logger);
            await CommonHelper.InfoAsync($"An error occurred: {ex.Message}", logger);
            return res;
        }
    }

}
