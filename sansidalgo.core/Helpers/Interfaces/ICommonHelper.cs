using sansidalgo.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sansidalgo.core.Helpers.Interfaces
{
    internal interface ICommonHelper
    {
       
        public Task<string> sha256_hash(string value);
        //public Task<OtpEntity> GetTOTP(string secretekey);
        public Task<string> GetOthersStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice);
        public Task<string> GetShoonyaStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice);
        public Task<string> GetStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice, string broker);
        public Task<string> GetFOAsset(Order order, string broker = "shoonya");
    }
}
