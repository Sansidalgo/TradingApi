using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenStreamMessage
{
	public string t;

	public virtual string toJson()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return JsonConvert.SerializeObject((object)this, new JsonSerializerSettings
		{
			NullValueHandling = (NullValueHandling)1
		});
	}
}
public class NorenStreamMessage<T> : BaseWSMessage where T : NorenStreamMessage, new()
{
	public OnStreamMesssage MessageHandler;

	public NorenStreamMessage(OnStreamMesssage Response)
	{
		MessageHandler = Response;
	}
}
