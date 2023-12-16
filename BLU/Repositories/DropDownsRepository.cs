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
    public class DropDownsRepository : BaseRepository, IBrokersRepository
    {
        public DropDownsRepository(AlgoContext _context) : base(_context)
        {
        }


        public async Task<DbStatus> GetBrokers()
        {
            DbStatus res = new DbStatus();

            //try
            //{
            //    DropDownsResponseDto ddr = new DropDownsResponseDto();
            //    ddr.Borkers =  context.TblBrokers;

            //    var Result =new
            //    {
            //        ,
            //        await context.TblBrokers.ToListAsync()
            //    }



            //    if (Result.Count > 0)
            //    {

            //        res.Result = Result;
            //        res.Status = 1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    res.Status = 0;
            //    res.Message = res.GetStatus(ex);
            //}
            return res;
        }
    }
}
