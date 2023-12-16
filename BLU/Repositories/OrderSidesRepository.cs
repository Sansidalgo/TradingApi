﻿using BLU.Enums;
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
    public class OrderSidesRepository : BaseRepository, IOrderSidesRepository
    {
        public OrderSidesRepository(AlgoContext _context) : base(_context)
        {
        }

        public async Task<DbStatus> GetOrderSides()
        {
            DbStatus res = new DbStatus();

            try
            {

                var Result = await context.TblOrderSides.ToListAsync();



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
