using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using sansidalgo.core.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class OrderSettingsRepository : BaseRepository, IOrderSettingsRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        public OrderSettingsRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> Add(OrderSettingsRequestDto settings, IMapper mapper)
        {
            DbStatus res = new DbStatus();
            var optionsSettings = new TblOptionsSetting();
            var shoonyaCred = new TblShoonyaCredential();
            var strategy = new TblStrategy();
       
            settings.Credential.Uid = await CommonHelper.EncodeValueAsync(settings.Credential.Uid);
            settings.Credential.Password = await CommonHelper.EncodeValueAsync(settings.Credential.Password);
            try
            {

                if (settings != null && settings.TraderId != 0)
                {
                    optionsSettings.TraderId = settings.TraderId;
                    shoonyaCred.TraderId = settings.TraderId;

                    bool isNewCredntialsorOptionSettings = false;

                    #region handlingcredentials insertion
                    if (settings.CredentialsID == 0 && settings?.Credential != null && settings?.Credential.Id <= 0)
                    {
                        shoonyaCred = mapper.Map<TblShoonyaCredential>(settings.Credential);
                        shoonyaCred.TraderId = settings.TraderId;
                        await context.TblShoonyaCredentials.AddAsync(shoonyaCred);
                        isNewCredntialsorOptionSettings = true;
                    }
                    else if (settings.CredentialsID > 0)
                    {
                        shoonyaCred.Id = settings.CredentialsID;
                    }
                    else if (settings.Credential.Id > 0)
                    {
                        shoonyaCred = mapper.Map<TblShoonyaCredential>(settings.Credential);
                        shoonyaCred.TraderId = settings.TraderId;
                        context.TblShoonyaCredentials.Update(shoonyaCred);

                        isNewCredntialsorOptionSettings = true;
                    }
                    #endregion
                    #region hadlingOptionSettings
                    if (settings.OptionsSettingsId == 0 && settings.OptionsSetting != null && settings.OptionsSetting.Id <= 0)
                    {
                        optionsSettings = mapper.Map<TblOptionsSetting>(settings.OptionsSetting);
                        optionsSettings.TraderId = settings.TraderId;
                        await context.TblOptionsSettings.AddAsync(optionsSettings);
                        isNewCredntialsorOptionSettings = true;
                    }
                    else if (settings.OptionsSettingsId > 0)
                    {

                        optionsSettings.Id = settings.OptionsSettingsId;
                    }
                    else if (settings.OptionsSetting.Id > 0)
                    {
                        optionsSettings = mapper.Map<TblOptionsSetting>(settings.OptionsSetting);
                        optionsSettings.TraderId = settings.TraderId;
                        context.TblOptionsSettings.Update(optionsSettings);

                        isNewCredntialsorOptionSettings = true;
                    }
                    #endregion

                    #region handlingStrategies

                    if (settings.StrategyId == 0 && settings?.StrategyName != null)
                    {
                        strategy.Name = settings.StrategyName;
                        strategy.TraderId = settings.TraderId;
                        await context.TblStrategies.AddAsync(strategy);
                        isNewCredntialsorOptionSettings = true;
                    }
                    else if (settings.StrategyId > 0 && !string.IsNullOrWhiteSpace(settings.StrategyName))
                    {
                        strategy.TraderId = settings.TraderId;
                        strategy.Name = settings.StrategyName;
                        strategy.Id = settings.StrategyId;
                        context.TblStrategies.Update(strategy);

                        isNewCredntialsorOptionSettings = true;

                    }
                    else if (settings.StrategyId > 0)
                    {
                        strategy.Id = settings.StrategyId;

                    }

                    #endregion

                    if (isNewCredntialsorOptionSettings)
                    {
                        isNewCredntialsorOptionSettings = Convert.ToBoolean(await context.SaveChangesAsync());


                    }

                    if (!settings.IsEditing)
                    {
                        var orderSettings = new TblOrderSetting();
                        orderSettings.Name = settings.Name;
                        orderSettings.TraderId = settings.TraderId;
                        orderSettings.BrokerId = settings.BrokerId;
                        orderSettings.OrderSideId = settings.OrderSideId;
                        orderSettings.BrokerCredentialsId = shoonyaCred.Id;
                        orderSettings.OptionsSettingsId = optionsSettings.Id;
                        orderSettings.StrategyId = strategy.Id;
                        orderSettings.EnvironmentId = settings.EnvironmentId;

                        await context.TblOrderSettings.AddAsync(orderSettings);

                        res.Status = await context.SaveChangesAsync();
                        if (res.Status != 1)
                        {
                            res.Message = "Entry unsuccessful";
                        }
                        else { res.Message = "Entry Successfull"; };
                    }
                    else if (isNewCredntialsorOptionSettings && settings.IsEditing)
                    {

                        var orderSettings = context.TblOrderSettings.Find(settings.Id);
                        if (orderSettings != null)
                        {
                            if (!string.IsNullOrWhiteSpace(settings.Name) && orderSettings.Name != settings.Name)
                            {
                                orderSettings.Name = settings.Name;
                            }
                        }

                        orderSettings.TraderId = settings.TraderId;
                        orderSettings.BrokerId = settings.BrokerId;
                        orderSettings.OrderSideId = settings.OrderSideId;
                        orderSettings.BrokerCredentialsId = shoonyaCred.Id;
                        orderSettings.OptionsSettingsId = optionsSettings.Id;
                        orderSettings.EnvironmentId = settings.EnvironmentId;
                        context.TblOrderSettings.Update(orderSettings);

                        res.Status = await context.SaveChangesAsync();
                        if (res.Status != 1)
                        {
                            res.Message = "Entry unsuccessful";
                        }
                        else { res.Message = "Entry Successfull"; };
                    }
                }



            }
            catch (Exception ex)
            {

                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }

        public async Task<DbStatus> Delete(int? orderSettingId)
        {
            DbStatus res = new DbStatus();
            try
            {
                var orderSetting = context.TblOrderSettings.Find(orderSettingId);
                context.TblOrderSettings.Remove(orderSetting);

                res.Status = await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Message = res.GetStatus(ex);


            }

            return res;
        }

        public async Task<DbStatus> GetOrderSettings(int? traderID)
        {
            DbStatus res = new DbStatus();
            if (traderID == null) { return res; }
            try
            {

                var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                                                           .Include(i => i.BrokerCredentials)
                                                           .Include(i => i.Broker)
                                                           .Include(i => i.OrderSide)
                                                           .Include(i => i.Environment)
                                                           .Include(i => i.Strategy)
                                                           .Where(w => w.TraderId == Convert.ToInt32(traderID))
                                                           .Select(s => new OrderSettingsResponseDto()
                                                           {
                                                               Id = s.Id,
                                                               Name = s.Name,

                                                               StrategyName = s.Strategy.Name,
                                                               EnvironmentName = s.Environment.Name,
                                                               InstrumentName = s.OptionsSettings.Instrument.Name,
                                                               BrokerName = s.Broker.Name,
                                                               OrderSideName = s.OrderSide.Name,
                                                               CredentialsName = s.BrokerCredentials.Name,
                                                               OptionsSettingsName = s.OptionsSettings.Name
                                                           })
                                                           .ToListAsync();



                if (Result.Count() > 0)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                logger.Error(ex);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
        public async Task<DbStatus> GetMasterTraderIdandOrderSideById(int? orderSettingId)
        {

            DbStatus res = new DbStatus();
            if (orderSettingId == null) { return res; }
            try
            {

                var Result = await context.TblOrderSettings .Include(i => i.Trader).Include(i => i.OrderSide)

                    .Where(w => w.Id == Convert.ToInt32(orderSettingId))
                    .Select(s=> new DelegateReponseDto() { TraderId = s.Trader.Id, OrderSideId = s.OrderSide.Id })
                    .FirstOrDefaultAsync();

                    res.Result = Result;
                    res.Status = 1;
            
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                logger.Error(ex);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
        public async Task<DbStatus> GetOrderSettingsByMasterId(List<int?> traderIds,int orderSideId)
        {

            DbStatus res = new DbStatus();
            if (traderIds == null || traderIds.Count<=0) { return res; }
            try
            {

                var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                    .Include(i => i.BrokerCredentials)
                    .Include(i => i.Broker)
                     .Include(i => i.Trader)
                     .Include(i => i.OrderSide)
                        .Include(i => i.Environment)
                        .Include(i => i.OptionsSettings.Instrument)
                        .Include(i => i.Strategy)
                    .Where(w=>w.OrderSide.Id==orderSideId && traderIds.Contains(w.Trader.Id) )
                    .Select(s => new OrderSettingsResponseDto() { Strategy = s.Strategy, Environment = s.Environment, Trader = s.Trader, Credential = s.BrokerCredentials, OptionsSetting = s.OptionsSettings, Broker = s.Broker, OrderSide = s.OrderSide, Name = s.Name, Id = s.Id, TraderId = s.Trader.Id })
                    .ToListAsync();



                if (Result != null)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                logger.Error(ex);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;


        }

        public async Task<DbStatus> GetOrderSettingsById(int? orderSettingId)
        {

            DbStatus res = new DbStatus();

            res.Status = 0;
            if (orderSettingId == null) { res.Message = "order setting id should not be empty"; return res; }
            try
            {

                var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                    .Include(i => i.BrokerCredentials)
                    .Include(i => i.Broker)
                     .Include(i => i.Trader)
                     .Include(i => i.OrderSide)
                        .Include(i => i.Environment)
                        .Include(i => i.OptionsSettings.Instrument)
                        .Include(i => i.Strategy)
                    .Where(w => w.Id == Convert.ToInt32(orderSettingId))
                    .Select(s => new OrderSettingsResponseDto() { Strategy = s.Strategy, Environment = s.Environment, Trader = s.Trader, Credential = s.BrokerCredentials, OptionsSetting = s.OptionsSettings, Broker = s.Broker, OrderSide = s.OrderSide, Name = s.Name, Id = s.Id,TraderId=s.Trader.Id })
                    .FirstOrDefaultAsync();



                if (Result != null)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                logger.Error(ex);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
        public async Task<DbStatus> GetOrderSettingsByIdForApi(int? orderSettingId)
        {

            DbStatus res = new DbStatus();
            if (orderSettingId == null) { return res; }
            try
            {

                var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                    .Include(i => i.BrokerCredentials)
                    .Include(i => i.Broker)
                     .Include(i => i.Trader)
                     .Include(i => i.OrderSide)
                        .Include(i => i.Environment)
                        .Include(i => i.OptionsSettings.Instrument)
                        .Include(i => i.Strategy)
                    .Where(w => w.Id == Convert.ToInt32(orderSettingId))
                    .Select(s => new OrderSettingsResponseDto() { Credential = CommonHelper.DecodeValues(s.BrokerCredentials), OptionsSetting = s.OptionsSettings, StrategyName = s.Strategy.Name, StrategyId = s.Strategy.Id, EnvironmentId = s.Environment.Id, CredentialsID = s.BrokerCredentialsId, OptionsSettingsId = s.OptionsSettingsId, BrokerId = s.Broker.Id, OrderSideId = s.OrderSide.Id, Name = s.Name, Id = s.Id })
                    .FirstOrDefaultAsync();



                if (Result != null)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                logger.Error(ex);
                res.Status = 0;
                
                res.Message = res.GetStatus(ex);
            }
            return res;
        }


    }
}
