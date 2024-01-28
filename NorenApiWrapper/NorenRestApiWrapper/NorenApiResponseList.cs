using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenApiResponseList<T, U> : BaseApiResponse where T : NorenListResponseMsg<U>, new()
{
	public delegate void ResponseNotify(NorenResponseMsg httpResponse);

	public ResponseNotify ResponseNotifyInstance;

	private OnResponse ResponseHandler;

	public NorenApiResponseList(OnResponse Response)
	{
		ResponseHandler = Response;
	}

	public override void OnMessageNotify(HttpResponseMessage httpResponse, string data)
	{
		//IL_00a6: Expected O, but got Unknown
		T val = new T();
		if (val == null)
		{
			return;
		}
		if (httpResponse.IsSuccessStatusCode)
		{
			try
			{
				if (data[0] != '[')
				{
					if (data[0] != '{')
					{
                        NorenResponseMsg norenMessage = GetNorenMessage(data);
                        val.Copy(norenMessage);
                        val.stat = "Not_Ok";
                        ResponseHandler(val, ok: false);
                        return;
                    }
					else if(data.Contains("Ok"))
					{
                        NorenResponseMsg norenMessage = GetNorenMessage(data);
                        val.Copy(norenMessage);
                        val.stat = "Ok";
                        ResponseHandler(val, ok: true);


                        return;
                    }
					
				}
				val.list = JsonConvert.DeserializeObject<List<U>>(data);
				val.stat = "Ok";
				val.request_time = "";
				val.emsg = "";
			}
			catch (JsonReaderException val2)
			{
				JsonReaderException val3 = val2;
				Console.WriteLine("Message Received " + data);
				Console.WriteLine("Error deserializing data " + ((object)val3).ToString());
				return;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error deserializing data " + ex.ToString());
				return;
			}
			ResponseNotifyInstance?.Invoke(val);
			ResponseHandler(val, ok: true);
		}
		else if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
		{
			NorenResponseMsg norenResponseMsg = new NorenResponseMsg();
			try
			{
				norenResponseMsg = JsonConvert.DeserializeObject<NorenResponseMsg>(data);
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Error deserializing data " + ex2.ToString());
				return;
			}
			val.stat = norenResponseMsg.stat;
			val.emsg = norenResponseMsg.emsg;
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
