namespace sansidalgo.core.Models
{
    public class Order
    {
        public string? UID { get; set; }
        public string? PSW { get; set; }
        public string? authSecretekey { get; set; }

        public string? imei { get; set; }
        public string? VC { get; set; }


        public string? ApiKey { get; set; }
        public string? peseudoAccount { get; set; }
        public int Quantity { get; set; }
        public int StrikePriceDifference { get; set; }
        public string? Exchange { get; set; }
        public decimal? IndexPrice { get; set; }
        public string? Asset { get; set; }
        public string? OrderType { get; set; }


        private string _ExpiryDay = "thursday";

        public string ExpiryDay
        {
            get { return _ExpiryDay; }
            set { _ExpiryDay = value; }
        }
        private string _startTime = "9:30";

        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        private string _endTime = "15:00";

        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }



        //            {
        //                "exchange": "{{exchange}}",
        //  "asset": "BANKNIFTY",
        //  "alertName": "psr_buy_entry",
        //  "strikePrice": "{{close}}",
        //  "quantity": "25"
        //}
    }
}
