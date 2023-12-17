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
    public interface IOrderSettingsRepository
    {
        public Task<DbStatus> Add(OrderSettingsRequestDto settings, IMapper mapper);
        public Task<DbStatus> GetOrderSettings(int? traderID);
        public Task<DbStatus> GetOrderSettingsById(int? orderSettingId);
        public Task<DbStatus> Delete(int? orderSettingId);
    }
}
