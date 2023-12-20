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
    public class StrategiesRepository : BaseRepository, IStrategiesRepository
    {
        public StrategiesRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> Add(StrategyResponseDto strategy, IMapper mapper)
        {

            DbStatus res = new DbStatus();

            try
            {
                

                var stategyObj = mapper.Map<TblStrategy>(strategy);
                context.TblStrategies.Add(stategyObj);

                res.Status = await context.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;

        }

        public async Task<DbStatus> GetStrategies(int traderId)
        {
            DbStatus res = new DbStatus();

            try
            {

               
               res.Result= await context.TblStrategies.Where(w=>w.TraderId==traderId).ToListAsync();
                res.Status = 1;

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
