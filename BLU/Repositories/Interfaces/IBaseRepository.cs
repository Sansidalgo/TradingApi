using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories.Interfaces
{
    public interface IBaseRepository  
    {
        AlgoContext context { get; }
    }
}
