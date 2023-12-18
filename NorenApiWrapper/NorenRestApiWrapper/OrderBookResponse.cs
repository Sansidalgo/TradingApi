using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class OrderBookResponse : NorenListResponseMsg<OrderBookItem>
{
	public List<OrderBookItem> Orders => list;
}
