using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class GetTPSeriesResponse : NorenListResponseMsg<TPSeriesItem>
{
	public List<TPSeriesItem> values => list;
}
