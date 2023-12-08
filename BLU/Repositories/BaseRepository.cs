using BLU.Repositories.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public abstract class BaseRepository
    {
        public readonly AlgoContext context;

        public BaseRepository(AlgoContext _context)
        {
            context = _context;
          
        }
       
    }
}
