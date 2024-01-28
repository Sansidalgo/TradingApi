namespace BLU.Dtos
{
    public class OptionContractResponseDto
    {
        public string Symbol { get; set; }
        public double StrikePrice { get; set; }
        public double OpenInterest { get; set; }
        public double DailyTradingRange { get; set; }
        public string OptionType { get; set; } // "CE" for Call, "PE" for Put
    }
}
