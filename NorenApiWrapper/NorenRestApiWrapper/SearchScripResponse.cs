using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class SearchScripResponse : NorenResponseMsg
{
	public List<ScripItem> values;

	public string request_time;
}
