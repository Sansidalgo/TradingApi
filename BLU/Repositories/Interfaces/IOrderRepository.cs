using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories.Interfaces
{
    public interface IOrderRepository
    {
      
        public Task<DbStatus> GetOrders(int traderID,int StatusId,int environmentId, DateTime startDate, DateTime endDate);
    }
}
