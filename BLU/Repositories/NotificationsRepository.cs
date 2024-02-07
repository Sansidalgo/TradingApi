using BLU.Enums;
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
    public class NotificationsRepository : BaseRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public NotificationsRepository(AlgoContext _context) : base(_context)
        { }
        public async Task<DbStatus> GetNotifications(int traderId)
        {
            DbStatus res = new DbStatus();
            try
            {
                var Result = await context.TblDelegates.Include(i => i.MasterTrader).Where(w => w.MasterTraderId == traderId && w.IsActive == false).Select(s => new { s.Id,s.TraderId,s.MasterTrader.Name,s.MasterTrader.EmailId }).ToListAsync();

                if (Result.Count() > 0)
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
    }
}
