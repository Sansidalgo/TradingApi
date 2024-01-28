using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class PositionBookResponse : NorenListResponseMsg<PositionBookItem>
{
	public List<PositionBookItem> positions => list;
}
