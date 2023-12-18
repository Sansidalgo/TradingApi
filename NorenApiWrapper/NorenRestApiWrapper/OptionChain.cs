using System.Web;

namespace NorenRestApiWrapper;

public class OptionChain : NorenMessage
{
	public string uid;

	public string exch;

	public string tsym;

	public string strprc;

	public string cnt;

	public override string toJson()
	{
		tsym = HttpUtility.UrlEncode(tsym);
		return base.toJson();
	}
}
