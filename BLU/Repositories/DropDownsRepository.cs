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

            
            return res;
        }
    }
}
