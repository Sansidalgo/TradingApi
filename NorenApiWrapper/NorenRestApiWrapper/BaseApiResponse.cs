using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class BaseApiResponse
{
	public virtual void OnMessageNotify(HttpResponseMessage httpResponse, string data)
	{
	}

	public NorenResponseMsg GetNorenMessage(string data)
	{
		NorenResponseMsg norenResponseMsg = new NorenResponseMsg();
		try
		{
			return JsonConvert.DeserializeObject<NorenResponseMsg>(data);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error deserializing data " + ex.ToString());
			return null;
		}
	}
}
