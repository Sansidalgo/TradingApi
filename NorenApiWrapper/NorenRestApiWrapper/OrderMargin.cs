using System.Web;

namespace NorenRestApiWrapper;

public class OrderMargin : NorenMessage
{
	public string uid;

	public string actid;

	public string exch;

	public string tsym;

	public string qty;

	public string prc;

	public string trgprc;

	public string dscqty;

	public string prd;

	public string trantype;

	public string prctyp;

	public string blprc;

	public string rorgqty;

	public string fillshares;

	public string rorgprc;

	public string orgtrgprc;

	public string norenordno;

	public string snonum;

	public override string toJson()
	{
		tsym = HttpUtility.UrlEncode(tsym);
		return base.toJson();
	}
}
