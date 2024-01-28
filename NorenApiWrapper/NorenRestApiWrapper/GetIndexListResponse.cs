using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class GetIndexListResponse : NorenResponseMsg
{
	public List<IndexListItem> values;

	public string request_time;
}
