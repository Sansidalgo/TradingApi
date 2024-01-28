using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NorenRestApiWrapper;

public class RESTClient
{
	private HttpClient client = new HttpClient();

	private string _endPoint;

	public OnResponse onSessionClose;

	public string endPoint
	{
		get
		{
			return _endPoint;
		}
		set
		{
			_endPoint = value;
			if (client.BaseAddress == null)
			{
				client.BaseAddress = new Uri(endPoint);
			}
		}
	}

	public RESTClient()
	{
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		client.DefaultRequestHeaders.ExpectContinue = false;
	}
    public async Task makeRequestAsync(BaseApiResponse response, string uri, string message, string key = null)
    {
        _ = string.Empty;
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        if (key != null)
        {
            httpRequestMessage.Content = new StringContent(message + "&" + key, Encoding.UTF8, "application/json");
        }
        else
        {
            httpRequestMessage.Content = new StringContent(message, Encoding.UTF8, "application/json");
        }
        Console.WriteLine("Request:" + uri + " " + message);
        await client.SendAsync(httpRequestMessage).ContinueWith((Func<Task<HttpResponseMessage>, Task>)async delegate (Task<HttpResponseMessage> responseTask)
        {
            _ = string.Empty;
            Console.WriteLine("Response: {0}", responseTask.Status);
            AggregateException? exception = responseTask.Exception;
            if (exception != null && exception.InnerExceptions?.Count > 0)
            {
                Console.WriteLine("Exception: {0}", responseTask.Exception.InnerException);
            }
            if (responseTask.IsCompleted)
            {
                string text = await responseTask.Result.Content.ReadAsStringAsync();
                Console.WriteLine("Response data: {0}", text);
                if (text == "{\"stat\":\"Not_Ok\",\"emsg\":\"Session Expired : Invalid Session Key\"}")
                {
                    LogoutResponse response2 = new LogoutResponse
                    {
                        emsg = "Session Expired : Invalid Session Key",
                        stat = "Not_Ok"
                    };
                    onSessionClose?.Invoke(response2, ok: false);
                }
                else
                {
                    response.OnMessageNotify(responseTask.Result, text);
                }
            }
        });
    }
    public async void makeRequest(BaseApiResponse response, string uri, string message, string key = null)
	{
		_ = string.Empty;
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
		if (key != null)
		{
			httpRequestMessage.Content = new StringContent(message + "&" + key, Encoding.UTF8, "application/json");
		}
		else
		{
			httpRequestMessage.Content = new StringContent(message, Encoding.UTF8, "application/json");
		}
		Console.WriteLine("Request:" + uri + " " + message);
		await client.SendAsync(httpRequestMessage).ContinueWith((Func<Task<HttpResponseMessage>, Task>)async delegate(Task<HttpResponseMessage> responseTask)
		{
			_ = string.Empty;
			Console.WriteLine("Response: {0}", responseTask.Status);
			AggregateException? exception = responseTask.Exception;
			if (exception != null && exception.InnerExceptions?.Count > 0)
			{
				Console.WriteLine("Exception: {0}", responseTask.Exception.InnerException);
			}
			if (responseTask.IsCompleted)
			{
				string text = await responseTask.Result.Content.ReadAsStringAsync();
				Console.WriteLine("Response data: {0}", text);
				if (text == "{\"stat\":\"Not_Ok\",\"emsg\":\"Session Expired : Invalid Session Key\"}")
				{
					LogoutResponse response2 = new LogoutResponse
					{
						emsg = "Session Expired : Invalid Session Key",
						stat = "Not_Ok"
					};
					onSessionClose?.Invoke(response2, ok: false);
				}
				else
				{
					response.OnMessageNotify(responseTask.Result, text);
				}
			}
		});
	}
}
