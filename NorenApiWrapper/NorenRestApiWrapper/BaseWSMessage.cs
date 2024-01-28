namespace NorenRestApiWrapper;

public class BaseWSMessage
{
	public virtual void OnMessageNotify(byte[] Data, int Count, string MessageType)
	{
	}
}
