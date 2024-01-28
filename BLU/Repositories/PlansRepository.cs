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

    public class PlansRepository : BaseRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public PlansRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> GetPlans()
        {
            DbStatus res = new DbStatus();

            try
            {

                var Result = await context.TblPlans.ToListAsync();



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
