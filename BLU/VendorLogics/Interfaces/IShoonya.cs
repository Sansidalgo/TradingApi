using BLU.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.VendorLogics.Interfaces
{
    public interface IShoonya
    {
        public Task<string> ExecuteShoonyaOrder(OrderSettingsResponseDto order, decimal IndexPrice);
    }
}
