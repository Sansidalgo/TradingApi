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
    public class EnvironmentsRepository : BaseRepository, IEnvironmentsRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public EnvironmentsRepository(AlgoContext _context) : base(_context)
        {
        }
        public async Task<DbStatus> GetEnvironments()
        {
            DbStatus res = new DbStatus();

            try
            {

                var Result = await context.TblEnvironments.ToListAsync();
                if (Result.Count > 0)
                {

                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                res.Status = 0;
                await CommonHelper.LogExceptionAsync(ex,logger);
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
    }
}
