using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
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
    public class UserSubscriptionRepository : BaseRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public UserSubscriptionRepository(AlgoContext _context) : base(_context)
        {
        }
        public async Task<DbStatus> GetSubscriptions()
        {
            DbStatus res = new DbStatus();

            try
            {
                var subscriptionStatusID = (await context.TblSubscriptionStatuses.Where(w => w.Name == "Payment Submitted").FirstOrDefaultAsync()).Id;

                var Result = await context.TblUserSubscriptions.Include(i => i.TblPayments).Where(w => w.SubscriptionStatusId == subscriptionStatusID).ToListAsync();
                

                if (Result.Count > 0)
                {

                    res.Result = Result;
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

        public async Task<DbStatus> GetSubscriptions(int traderId)
        {
            DbStatus res = new DbStatus();

            try
            {

                var Result = await context.TblUserSubscriptions.Where(w=>w.TraderId==traderId).ToListAsync();



                if (Result.Count > 0)
                {

                    res.Result = Result;
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
        public async Task<DbStatus> AddSubscriptions(int traderId,int planId)
        {
            DbStatus res = new DbStatus();

            try
            {

                var Result = await context.TblUserSubscriptions.Where(w => w.TraderId == traderId).ToListAsync();



                if (Result.Count > 0)
                {

                    res.Result = Result;
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
    }
}
