using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class MultiLegOrderBookResponse : NorenListResponseMsg<MultiLegOrderBookItem>
{
	public List<MultiLegOrderBookItem> mlorders => list;
}
