using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class UserDetailsResponse : NorenResponseMsg
{
	public List<string> exarr;

	public List<string> orarr;

	public List<ProductInfo> prarr;

	public string brkname;

	public string brnchid;

	public string email;

	public string actid;

	public string uprev;

	public string request_time;
}
