using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using sansidalgo.core.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class DelegateRepository : BaseRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public DelegateRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> GetDelegateByMasterId(int masterTraderId)
        {
            DbStatus res = new DbStatus();
            try
            {
                

                var traderIds = await context.TblDelegates.Where(w=>w.MasterTraderId== masterTraderId && w.IsActive==true).Select(s=>s.TraderId).ToListAsync();




                if (traderIds.Count > 0)
                {

                    res.Result = traderIds;
                    res.Status = 1;
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

        public async Task<DbStatus> AddDelegate(int traderId,TblDelegate delegateDetails, IMapper mapper)
        {
            DbStatus res = new DbStatus();
            //var delegateSettings = new TblDelegate();
            try
            {
                //check if requester is using his own id to delegate
                
                //check if MasterID exists
                var traderDetails = await context.TblTraderDetails.Where(w => w.Id == delegateDetails.MasterTraderId).Select(s => new { s.Id, s.Name }).FirstOrDefaultAsync();
                if (traderDetails != null && traderId!=delegateDetails.MasterTraderId)
                {
                    var delegateObj = mapper.Map<TblDelegate>(delegateDetails);
                    context.TblDelegates.Add(delegateObj);
                    res.Message = "Delegate details updated successfully";
                    res.Result = traderDetails;
                    res.Status = await context.SaveChangesAsync();
                }
                else
                {
                    res.Message = "Master\\Delegate ID doesnt exist";
                    res.Status = 0;
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

        public async Task<DbStatus> GetDelegates(int traderId)
        {
            DbStatus res = new DbStatus();
            try
            {
                var Result = await context.TblDelegates.Include(i=>i.MasterTrader).Where(w => w.TraderId == traderId).Select(s => new { s.Id,s.MasterTrader.Name}).FirstOrDefaultAsync();

                if (Result!=null)
                {

                    res.Result = Result;
                    res.Status = 1;
                    //res.Message = Convert.ToString(Result.First().MasterTraderId);
                }
                else
                {
                    res.Result = null;
                    res.Status = 0;
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

        public async Task<DbStatus> Delete(int Id)
        {
            DbStatus res = new DbStatus();
            try
            {
                var removeDelegateSetting = context.TblDelegates.Find(Id);
                context.TblDelegates.Remove(removeDelegateSetting);

                res.Status = await context.SaveChangesAsync();
                res.Message = "Delegate removed successfully";
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Message = res.GetStatus(ex);


            }

            return res;
        }
    }
}
