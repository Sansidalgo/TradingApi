using NorenRestApiWrapper;
using sansidalgo.core.Helpers.Interfaces;
using sansidalgo.core.Models;
using System.Security.Cryptography;
using System.Text;

namespace sansidalgo.core.helpers
{
    public class CommonHelper : ICommonHelper
    {
        public async Task<string> EncodeValue(string value)
        {
            value = string.Concat(value, "01B718E1348642199422B0D8DBC0A6BD");
            return await Task.FromResult(Convert.ToBase64String(Encoding.UTF8.GetBytes(value)));
        }

        public async Task<string> DecodeValue(string value)
        {
            return await Task.FromResult(Encoding.UTF8.GetString(Convert.FromBase64String(value)).Replace("01B718E1348642199422B0D8DBC0A6BD", string.Empty));
        }
        public async Task<string> sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = await Task.FromResult(hash.ComputeHash(enc.GetBytes(value)));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public async Task<string> GetTOTP(string secretekey)
        {

            var bytes = OtpNet.Base32Encoding.ToBytes(secretekey);

            var totp = new OtpNet.Totp(bytes, 30, OtpNet.OtpHashMode.Sha1);

            var totpcode = await Task.FromResult(totp.ComputeTotp());
            var remainingTime = totp.RemainingSeconds();
            return totpcode;
        }
        public async Task<string> GetOthersStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice)
        {

            optionType = optionType.ToUpper();
            DateTime currentDateTime = DateTime.Now;
            var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekString, true);

            // Calculate the expiry date for the current week (Thursday)
            DateTime expiryDate = currentDateTime.AddDays(dayOfWeek - currentDateTime.DayOfWeek);

            // Adjust expiry date if current day is after Thursday
            if (currentDateTime.DayOfWeek > dayOfWeek)
            {
                expiryDate = expiryDate.AddDays(7);
            }

            // Prepare the strike price symbol
            string strikePriceSymbol = symbol + "_" + $"{expiryDate:dd-MMM-yyyy}" + "_" + optionType + "_" + strikePrice;
            return await Task.FromResult(strikePriceSymbol.ToUpper());
        }
        public async Task<string> GetShoonyaStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice)
        {

            optionType = optionType.ToUpper();
            DateTime currentDateTime = DateTime.Now;
            var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekString, true);

            // Calculate the expiry date for the current week (Thursday)
            DateTime expiryDate = currentDateTime.AddDays(dayOfWeek - currentDateTime.DayOfWeek);

            // Adjust expiry date if current day is after Thursday
            if (currentDateTime.DayOfWeek > dayOfWeek)
            {
                expiryDate = expiryDate.AddDays(7);
            }

            // Prepare the strike price symbol
            string strikePriceSymbol = symbol + $"{expiryDate:ddMMMyy}" + optionType + strikePrice;
            return await Task.FromResult(strikePriceSymbol.ToUpper());
        }
        public async Task<string> GetStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice, string broker)
        {
            string tSymbol = string.Empty;
            switch (broker)
            {
                case "shoonya":
                    if (optionType == "CE")
                    {
                        optionType = "C";
                    }
                    else
                    {
                        optionType = "P";
                    }
                    tSymbol = await GetShoonyaStrikePrice(dayOfWeekString, symbol, optionType, strikePrice);
                    break;

                default:
                    tSymbol = await GetShoonyaStrikePrice(dayOfWeekString, symbol, optionType, strikePrice);
                    break;
            }
            return tSymbol;
        }
        public async Task<string> GetFOAsset(Order order, string broker = "shoonya")
        {
            order.Asset = order.Asset.ToUpper();
            string asset = string.Empty;
            int strikePrice = (int)Math.Round((double)order?.IndexPrice / order.StrikePriceDifference) * order.StrikePriceDifference;
            if (order.OrderType == "ce_entry")
            {
                strikePrice = (int)strikePrice + order.StrikePriceDifference;
            }
            if (order.OrderType == "pe_entry")
            {
                strikePrice = (int)strikePrice - order.StrikePriceDifference;
            }

            if (order.Asset == "BANKNIFTY" && order.OrderType == "pe_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "PE", strikePrice, broker);
            }
            else if (order.Asset == "BANKNIFTY" && order.OrderType == "ce_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "CE", strikePrice, broker);
            }
            if (order.Asset == "NIFTY" && order.OrderType == "pe_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "PE", strikePrice, broker);
            }
            else if (order.Asset == "NIFTY" && order.OrderType == "ce_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "CE", strikePrice, broker);
            }
            else if (order.OrderType == "ce_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order?.Asset, "CE", strikePrice, broker);
            }
            else if (order.OrderType == "pe_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order?.Asset, "PE", strikePrice, broker);
            }
            return asset;
        }


    }
}
