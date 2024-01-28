using BLU.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories.Interfaces
{
    public interface IOptionsSettingsRepository
    {
        public Task<DbStatus> Add(TblOptionsSetting settings);
        public Task<DbStatus> GetOptionsSettings(int? traderID);
    }
}
