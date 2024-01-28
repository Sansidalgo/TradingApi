using System.Web;

namespace NorenRestApiWrapper;

public class ModifyOrder : NorenMessage
{
	public string exch;

	public string norenordno;

	public string prctyp;

	public string prc;

	public string qty;

	public string tsym;

	public string ret;

	public string trgprc;

	public string uid;

	public string bpprc;

	public string blprc;

	public string trailprc;

	public override string toJson()
	{
		tsym = HttpUtility.UrlEncode(tsym);
		return base.toJson();
	}
}
