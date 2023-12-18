using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenApiResponse<T> : BaseApiResponse where T : NorenResponseMsg, new()
{
	public delegate void ResponseNotify(NorenResponseMsg httpResponse);

	public ResponseNotify ResponseNotifyInstance;

	private OnResponse ResponseHandler;

	public NorenApiResponse(OnResponse Response)
	{
		ResponseHandler = Response;
	}

	public override void OnMessageNotify(HttpResponseMessage httpResponse, string data)
	{
		//IL_004d: Expected O, but got Unknown
		T val = new T();
		if (val == null)
		{
			return;
		}
		if (httpResponse.IsSuccessStatusCode)
		{
			try
			{
				val = JsonConvert.DeserializeObject<T>(data);
				ResponseNotifyInstance?.Invoke(val);
				ResponseHandler(val, ok: true);
				return;
			}
			catch (JsonReaderException val2)
			{
				JsonReaderException val3 = val2;
				Console.WriteLine("Error deserializing data " + ((object)val3).ToString());
				return;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error deserializing data " + ex.ToString());
				return;
			}
		}
		if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
		{
			NorenResponseMsg norenMessage = GetNorenMessage(data);
			val.stat = norenMessage.stat;
			val.emsg = norenMessage.emsg;
			ResponseHandler(val, ok: false);
		}
		else
		{
			val.stat = httpResponse.StatusCode.ToString();
			val.emsg = data;
			ResponseHandler(val, ok: false);
		}
	}
}
