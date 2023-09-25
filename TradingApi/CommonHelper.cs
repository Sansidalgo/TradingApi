using NorenRestApiWrapper;
using System.Security.Cryptography;
using System.Text;

namespace TradingApi
{
    public class CommonHelper
    {
       
        public static String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static string GetTOTP(string secretekey)
        {
           
            var bytes = OtpNet.Base32Encoding.ToBytes(secretekey);

            var totp = new OtpNet.Totp(bytes, 30, OtpNet.OtpHashMode.Sha1);

            var totpcode = totp.ComputeTotp();
            var remainingTime = totp.RemainingSeconds();
            return totpcode;
        }
        public static async Task<string> GetOthersStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice)
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
        public static async Task<string> GetShoonyaStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice)
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
            string strikePriceSymbol = symbol+ $"{expiryDate:ddMMMyy}"+ optionType + strikePrice;
            return await Task.FromResult(strikePriceSymbol.ToUpper());
        }
        public static async Task<string> GetStrikePrice(string dayOfWeekString, string symbol, string optionType, decimal strikePrice,string broker)
        {
            string tSymbol = string.Empty;
            switch (broker)
            {
                case "shoonya":
                    if(optionType=="CE")
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
        public static async Task<string> GetFOAsset(Order order,string broker="shoonya")
        {
            string asset = string.Empty;
            int strikePrice = (int)Math.Round((double)order.IndexPrice / order.StrikePriceDifference) * order.StrikePriceDifference;
            if (order.OrderType == "ce_entry")
            {
                strikePrice = (int)strikePrice + order.StrikePriceDifference;
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
                asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "CE", strikePrice, broker);
            }
            else if (order.OrderType == "pe_entry")
            {
                asset = await GetStrikePrice(order.ExpiryDay, order.Asset, "PE", strikePrice,broker);
            }
            return asset;
        }
    }
}
