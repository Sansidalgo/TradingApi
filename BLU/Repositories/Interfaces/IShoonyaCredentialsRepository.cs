using Azure.Core.Pipeline;
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
    public interface IShoonyaCredentialsRepository
    {
        public  Task<DbStatus> Add(TblShoonyaCredential credential);
       public Task<DbStatus> GetCredentials(int? traderID);
        

    }
}
