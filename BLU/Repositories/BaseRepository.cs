using BLU.Repositories.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BLU.Repositories
{

    public class BaseRepository : IBaseRepository
    {
        public AlgoContext context { get; }

        public BaseRepository(AlgoContext _context)
        {
            context = _context;
        }
    }
}
