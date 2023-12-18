using System;
using System.Text;
using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenWebSocket
{
	private WebSocket _ws = new WebSocket();

	private string _uid;

	private string _susertoken;

	private string _endpoint;

	public OnStreamConnect onStreamConnectCallback;

	public OnCloseHandler onStreamCloseCallback;

	public OnErrorHandler onStreamErrorCallback;

	public OnFeed OnFeedCallback;

	public OnOrderFeed OnOrderCallback;

	public bool IsConnected => _ws.IsConnected();

	public NorenWebSocket()
	{
		_ws.OnConnect += _onConnect;
		_ws.OnData += _onData;
		_ws.OnClose += _onClose;
		_ws.OnError += _onError;
	}

	public void Start(string url, string uid, string susertoken, OnFeed marketdataHandler, OnOrderFeed orderHandler)
	{
		_endpoint = url;
		_uid = uid;
		_susertoken = susertoken;
		OnFeedCallback = marketdataHandler;
		OnOrderCallback = orderHandler;
		_ws.Connect(_endpoint);
	}

	public void Stop()
	{
		_ws.Close();
	}

	private void _onError(string Message)
	{
		Console.WriteLine("Error websocket: " + Message);
		onStreamErrorCallback?.Invoke(Message);
	}

	private void _onClose()
	{
		Console.WriteLine("websocket closed");
		onStreamCloseCallback?.Invoke();
	}

	private void _onConnect()
	{
		ConnectMessage connectMessage = new ConnectMessage();
		connectMessage.t = "c";
		connectMessage.uid = _uid;
		connectMessage.actid = _uid;
		connectMessage.susertoken = _susertoken;
		_ws.Send(connectMessage.toJson());
		Console.WriteLine("Create Session: " + connectMessage.toJson());
	}

	public void Send(string data)
	{
		if (_ws.IsConnected())
		{
			_ws.Send(data);
		}
		else
		{
			Console.WriteLine("send failed as websocket is not connected: " + data);
		}
	}

	public static T Deserialize<T>(byte[] data, int count) where T : class
	{
		return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data, 0, count));
	}

	private void _onData(byte[] Data, int Count, string MessageType)
	{
		//IL_009d: Expected O, but got Unknown
		try
		{
			if (Count != 0)
			{
				NorenStreamMessage norenStreamMessage = Deserialize<NorenStreamMessage>(Data, Count);
				if (norenStreamMessage.t == "ck")
				{
					Console.WriteLine("session established");
					onStreamConnectCallback?.Invoke(norenStreamMessage);
				}
				else if (norenStreamMessage.t == "om" || norenStreamMessage.t == "ok")
				{
					NorenOrderFeed feed = Deserialize<NorenOrderFeed>(Data, Count);
					OnOrderCallback?.Invoke(feed);
				}
				else
				{
					NorenFeed feed2 = Deserialize<NorenFeed>(Data, Count);
					OnFeedCallback?.Invoke(feed2);
				}
			}
		}
		catch (JsonReaderException val)
		{
			JsonReaderException val2 = val;
			Console.WriteLine("Error deserializing data " + ((object)val2).ToString());
			onStreamErrorCallback?.Invoke(((object)val2).ToString());
		}
	}
}
