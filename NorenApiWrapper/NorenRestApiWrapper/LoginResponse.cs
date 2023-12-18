using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class LoginResponse : NorenResponseMsg
{
	public string request_time;

	public string susertoken;

	public string lastaccesstime;

	public string spasswordreset;

	public List<string> exarr;

	public string uname;

	public List<ProductInfo> prarr;

	public string actid;

	public string email;

	public string brkname;
}
