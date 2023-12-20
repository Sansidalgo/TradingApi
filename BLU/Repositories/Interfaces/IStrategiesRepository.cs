using AutoMapper;
using BLU.Dtos;
using BLU.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories.Interfaces
{
    public interface IStrategiesRepository
    {
        public Task<DbStatus> GetStrategies(int traderId);
        public Task<DbStatus> Add(StrategyResponseDto strategy, IMapper mapper);
    }
}
