using AutoMapper;
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

        public async Task<DbStatus> Add(OrderSettingsRequestDto settings,IMapper mapper)
        {
            DbStatus res = new DbStatus();
            var optionsSettings = new TblOptionsSetting();
            var shoonyaCred = new TblShoonyaCredential();
            try
            {

                if (settings !=null && settings.TraderId != 0)
                {
                   optionsSettings.TraderId=settings.TraderId;
                    shoonyaCred.TraderId=settings.TraderId;

                    bool isNewCredntialsorOptionSettings = false;
                    if (settings.CredentialsID == 0 && settings?.Credential != null && settings?.Credential.Id<=0)
                    {
                        shoonyaCred = mapper.Map<TblShoonyaCredential>(settings.Credential);
                        await context.TblShoonyaCredentials.AddAsync(shoonyaCred);
                        isNewCredntialsorOptionSettings = true;
                    }
                    else if(settings.CredentialsID>0)
                    {
                        shoonyaCred.Id= settings.CredentialsID;
                    }
                    else if(settings.Credential.Id>0)
                    {
                        shoonyaCred = mapper.Map<TblShoonyaCredential>(settings.Credential);
                         context.TblShoonyaCredentials.Update(shoonyaCred);
                        
                        isNewCredntialsorOptionSettings = true;
                    }


                    if (settings.OptionsSettingsId == 0 && settings.OptionsSetting != null  && settings.OptionsSetting.Id<=0)
                    {
                        optionsSettings = mapper.Map<TblOptionsSetting>(settings.OptionsSetting);
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
                         context.TblOptionsSettings.Update(optionsSettings);
                     
                        isNewCredntialsorOptionSettings = true;
                    }



                    if (isNewCredntialsorOptionSettings)
                    {
                        isNewCredntialsorOptionSettings=Convert.ToBoolean(await context.SaveChangesAsync());
                        

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
                        if (orderSettings != null )
                        {
                            if(!string.IsNullOrWhiteSpace(settings.Name) && orderSettings.Name != settings.Name)
                            {
                                orderSettings.Name = settings.Name;
                            }
                        }

                        orderSettings.TraderId = settings.TraderId;
                        orderSettings.BrokerId = settings.BrokerId;
                        orderSettings.OrderSideId = settings.OrderSideId;
                        orderSettings.BrokerCredentialsId = shoonyaCred.Id;
                        orderSettings.OptionsSettingsId = optionsSettings.Id;

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

                var Result = await context.TblOrderSettings.Include(i => i.OptionsSettings)
                                                           .Include(i => i.BrokerCredentials)
                                                           .Include(i => i.Broker)
                                                           .Include(i=>i.OrderSide)
                                                           .Where(w => w.TraderId == Convert.ToInt32(traderID))
                                                           .Select(s => new OrderSettingsResponseDto()
                                                           {
                                                               Id = s.Id,
                                                               Name = s.Name,
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
                    .Select(s => new OrderSettingsResponseDto() { Credential = s.BrokerCredentials, OptionsSetting = s.OptionsSettings, Broker = s.Broker,OrderSide=s.OrderSide,Name=s.Name,Id=s.Id })
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
