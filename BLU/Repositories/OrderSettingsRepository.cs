using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class OrderSettingsRepository : BaseRepository, IOrderSettingsRepository
    {
        public OrderSettingsRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> Add(OrderSettingsRequestDto settings)
        {
            DbStatus res = new DbStatus();
            var optionsSettings = new TblOptionsSetting();
            var shoonyaCred = new TblShoonyaCredential();
            try
            {

                if (settings !=null && settings.TraderId != 0)
                {
                   

                    bool isNewCredntialsorOptionSettings = false;
                    if (settings.CredentialsID == 0 && settings?.Credential != null)
                    {
                        var dto = settings?.Credential;
                        dto.TraderId = settings.TraderId ?? 0;

                        shoonyaCred.Name= dto.Name;
                        shoonyaCred.TraderId = dto.TraderId;
                        shoonyaCred.Uid = dto.Uid;
                        shoonyaCred.Password = dto.Password;
                        shoonyaCred.AuthSecreteKey = dto.AuthSecreteKey;
                        shoonyaCred.Imei = dto.Imei;
                        shoonyaCred.Vc = dto.Vc;
                        shoonyaCred.ApiKey = dto.ApiKey;
                        shoonyaCred.IsActive = dto.IsActive;
                       
                        await context.TblShoonyaCredentials.AddAsync(shoonyaCred);
                        isNewCredntialsorOptionSettings = true;
                    }
                    if (settings.OptionsSettingsId == 0 && settings.OptionsSetting != null)
                    {
                        var dto = settings?.OptionsSetting;
                        dto.TraderId = settings.TraderId??0;

                       
                        optionsSettings.Instrument = dto.Instrument;
                        optionsSettings.ExpiryDay = dto.ExpiryDay;
                        optionsSettings.LotSize = dto.LotSize;
                        optionsSettings.CeSideEntryAt = dto.CeSideEntryAt;
                        optionsSettings.PeSideEntryAt = dto.PeSideEntryAt;
                        optionsSettings.TraderId = dto.TraderId;
                        // Map other properties accordingly

                        await context.TblOptionsSettings.AddAsync(optionsSettings);
                        isNewCredntialsorOptionSettings = true;
                    }
                    if (isNewCredntialsorOptionSettings)
                    {
                        isNewCredntialsorOptionSettings=Convert.ToBoolean(await context.SaveChangesAsync());
                        

                    }

                    if (isNewCredntialsorOptionSettings)
                    {
                        var orderSettings = new TblOrderSetting();
                        orderSettings.TraderId = settings.TraderId ?? 0;
                        orderSettings.BrokerId = settings.BrokerId;
                        orderSettings.BrokerCredentialsId = shoonyaCred.Id;
                        orderSettings.OptionsSettingsId = optionsSettings.Id;
                        await context.TblOrderSettings.AddAsync(orderSettings);

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
                try
                {
                    context.TblShoonyaCredentials.Remove(shoonyaCred);
                    context.TblOptionsSettings.Remove(optionsSettings);
                    context.SaveChanges();
                }
                catch 
                {

                   
                }
                
                res.Status = 0;
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

                //var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                //    .Include(i => i.BrokerCredentials)
                //    .Include(i => i.Broker)
                //    .Where(w => w.TraderId == Convert.ToInt32(traderID))
                //    .Select(s => new OrderSettingsResponseDto() {Id=s.Id, Broker=s.Broker.Broker,CredentialsName=s.BrokerCredentials.Name,OptionsSettingsName=s.OptionsSettings.Name})
                //    .ToListAsync();



                //if (Result.Count > 0)
                //{

                //    res.Result = Result;
                //    res.Status = 1;
                //}
            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }

        public async Task<DbStatus> GetOrderSettingsById(int? orderSettingId)
        {
            DbStatus res = new DbStatus();
            if (orderSettingId == null) { return res; }
            try
            {

                var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                    .Include(i => i.BrokerCredentials)
                    .Include(i => i.Broker)
                    .Where(w => w.Id == Convert.ToInt32(orderSettingId))
                    .Select(s => new OrderSettingsResponseDto() { Credential = s.BrokerCredentials, OptionsSetting = s.OptionsSettings, BrokerDetails = s.Broker })
                    .FirstOrDefaultAsync();



                if (Result != null)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
    }
}
