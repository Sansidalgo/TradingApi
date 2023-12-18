using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class MWListResponse : NorenResponseMsg
{
	public List<string> values;

	public string request_time;
}
