using System.Web;

namespace NorenRestApiWrapper;

public class SearchScrip : NorenMessage
{
	public string uid;

	public string stext;

	public string exch;

	public override string toJson()
	{
		stext = HttpUtility.UrlEncode(stext);
		return base.toJson();
	}
}
