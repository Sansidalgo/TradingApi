using BLU.Dtos;
using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.VendorLogics.Interfaces
{
    public interface IShoonya
    {
        public Task<string> ExecuteShoonyaOrder(OrderSettingsResponseDto order, decimal IndexPrice, ShoonyaReponseDto shoonyaResponse);
    }
}
