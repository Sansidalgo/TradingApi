using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NorenRestApiWrapper;

internal class WebSocket
{
	private ClientWebSocket _ws;

	private string _url;

	private int _bufferLength;

	public event OnConnectHandler OnConnect;

	public event OnCloseHandler OnClose;

	public event OnDataHandler OnData;

	public event OnErrorHandler OnError;

	public WebSocket(int BufferLength = 2000000)
	{
		_bufferLength = BufferLength;
	}

	public bool IsConnected()
	{
		if (_ws == null)
		{
			return false;
		}
		return _ws.State == WebSocketState.Open;
	}

	public void Connect(string Url, Dictionary<string, string> headers = null)
	{
		_url = Url;
		try
		{
			_ws = new ClientWebSocket();
			_ws.Options.KeepAliveInterval = TimeSpan.Zero;
			if (headers != null)
			{
				foreach (string key in headers.Keys)
				{
					_ws.Options.SetRequestHeader(key, headers[key]);
				}
			}
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			cancellationTokenSource.CancelAfter(5000);
			_ws.ConnectAsync(new Uri(_url), cancellationTokenSource.Token).Wait();
		}
		catch (AggregateException ex)
		{
			foreach (string item in ex.InnerException.Messages())
			{
				this.OnError?.Invoke("Error while connecting. Message: " + item);
				if (item.Contains("Forbidden") && item.Contains("403"))
				{
					this.OnClose?.Invoke();
				}
			}
			return;
		}
		catch (Exception ex2)
		{
			this.OnError?.Invoke("Error while connecting. Message:  " + ex2.Message);
			return;
		}
		this.OnConnect?.Invoke();
		byte[] buffer = new byte[_bufferLength];
		Action<Task<WebSocketReceiveResult>> callback = null;
		try
		{
			callback = delegate(Task<WebSocketReceiveResult> t)
			{
				try
				{
					byte[] array = new byte[_bufferLength];
					int num = t.Result.Count;
					bool endOfMessage = t.Result.EndOfMessage;
					while (!endOfMessage)
					{
						WebSocketReceiveResult result = _ws.ReceiveAsync(new ArraySegment<byte>(array), CancellationToken.None).Result;
						Array.Copy(array, 0, buffer, num, result.Count);
						num += result.Count;
						endOfMessage = result.EndOfMessage;
					}
					this.OnData?.Invoke(buffer, num, t.Result.MessageType.ToString());
					_ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ContinueWith(callback);
				}
				catch (Exception ex4)
				{
					if (IsConnected())
					{
						this.OnError?.Invoke("Error while recieving data. Message:  " + ex4.Message);
					}
					else
					{
						this.OnError?.Invoke("Lost websocket connection.");
					}
				}
			};
			_ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ContinueWith(callback);
		}
		catch (Exception ex3)
		{
			this.OnError?.Invoke("Error while recieving data. Message:  " + ex3.Message);
		}
	}

	public void Send(string Message)
	{
		if (_ws.State == WebSocketState.Open)
		{
			try
			{
				_ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(Message)), WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None).Wait();
			}
			catch (Exception ex)
			{
				this.OnError?.Invoke("Error while sending data. Message:  " + ex.Message);
			}
		}
	}

	public void Close(bool Abort = false)
	{
		if (_ws.State != WebSocketState.Open)
		{
			return;
		}
		try
		{
			if (Abort)
			{
				_ws.Abort();
				return;
			}
			_ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).Wait();
			this.OnClose?.Invoke();
		}
		catch (Exception ex)
		{
			this.OnError?.Invoke("Error while closing connection. Message: " + ex.Message);
		}
	}
}
