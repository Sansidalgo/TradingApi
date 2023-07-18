namespace TradingApi
{
    public class Order
    {
        public string? ApiKey { get; set; }
        public string? peseudoAccount { get; set; }
        public int Quantity { get; set; }
        public int StrikePriceDifference { get; set; }
        public string? Exchange { get; set; }
        public decimal? IndexPrice { get; set; }
        public string? Asset { get; set; }
        public string? OrderType { get; set; }


        //            {
        //                "exchange": "{{exchange}}",
        //  "asset": "BANKNIFTY",
        //  "alertName": "psr_buy_entry",
        //  "strikePrice": "{{close}}",
        //  "quantity": "25"
        //}
    }
}
