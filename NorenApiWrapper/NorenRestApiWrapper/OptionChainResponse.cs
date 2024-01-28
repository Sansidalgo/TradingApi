using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class OptionChainResponse : NorenResponseMsg
{
	public List<OptionChainItem> values;

	public string request_time;
}
