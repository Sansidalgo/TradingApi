using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class MarketWatchResponse : NorenResponseMsg
{
	public List<MarketWatchItem> values;

	public string request_time;
}
