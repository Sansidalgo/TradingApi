using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class OptionsSettingsRepository : BaseRepository, IOptionsSettingsRepository
    {
        public OptionsSettingsRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> Add(TblOptionsSetting settings)
        {
            DbStatus res = new DbStatus();
            try
            {
                if (settings.TraderId != 0)
                {
                    await context.TblOptionsSettings.AddAsync(settings);

                    res.Status = await context.SaveChangesAsync();
                    if (res.Status != 1)
                    {
                        res.Message = "Entry unsuccessful";
                    }
                    else { res.Message = "Entry Successfull"; };
                }



            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
       
        public async Task<DbStatus> GetOptionsSettings(int? traderID)
        {
            DbStatus res = new DbStatus();
            if (traderID == null) { return res; }
            try
            {

                var Result = await context.TblOptionsSettings.Where(w => w.TraderId == Convert.ToInt32(traderID)).ToListAsync();



                if (Result.Count > 0)
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
