using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class OrderHistoryResponse : NorenListResponseMsg<SingleOrdHistItem>
{
	public List<SingleOrdHistItem> history => list;
}
