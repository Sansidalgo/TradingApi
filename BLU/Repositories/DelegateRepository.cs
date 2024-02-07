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
    }
}
