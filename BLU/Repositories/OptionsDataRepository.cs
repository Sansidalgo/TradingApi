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
    public class OptionsDataRepository: BaseRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public OptionsDataRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> GetMarketData()
        {
            DbStatus res = new DbStatus();

            try
            {

                // Get the Indian Standard Time (IST) zone
                TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                // Get the current date in Indian time zone
                DateTime indianNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, indianTimeZone);
                DateTime indianToday = indianNow.Date;

                // Query to check if EntryDateTime is today's date in Indian time zone
                var Result = await context.TblOptionsData
                    .Where(w => ((DateTime)w.EntryDateTime).Date == indianToday)
                    .ToListAsync();


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
