using System.Web;

namespace NorenRestApiWrapper;

public class PlaceOrder : NorenMessage
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

	public string ret;

	public string remarks;

	public string ordersource;

	public string bpprc;

	public string blprc;

	public string trailprc;

	public string amo;

	public string tsym2;

	public string trantype2;

	public string qty2;

	public string prc2;

	public string tsym3;

	public string trantype3;

	public string qty3;

	public string prc3;

	public override string toJson()
	{
		tsym = HttpUtility.UrlEncode(tsym);
		tsym2 = HttpUtility.UrlEncode(tsym2);
		tsym3 = HttpUtility.UrlEncode(tsym3);
		remarks = HttpUtility.UrlEncode(remarks);
		return base.toJson();
	}
}
