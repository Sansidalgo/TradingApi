using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class TradeBookResponse : NorenListResponseMsg<TradeBookItem>
{
	public List<TradeBookItem> trades => list;
}
