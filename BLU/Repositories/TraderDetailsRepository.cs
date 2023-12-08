using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class TraderDetailsRepository : BaseRepository, ITraderDetailsRepository
    {
        public TraderDetailsRepository(AlgoContext _context) : base(_context)
        {
        }

       

    }
}
