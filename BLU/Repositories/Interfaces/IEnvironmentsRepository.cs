﻿using BLU.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories.Interfaces
{
    public interface IEnvironmentsRepository
    {
        public Task<DbStatus> GetEnvironments();
    }
}
