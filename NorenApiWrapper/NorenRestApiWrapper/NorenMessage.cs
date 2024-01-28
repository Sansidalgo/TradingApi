using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenMessage
{
	public virtual string toJson()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		string text = JsonConvert.SerializeObject((object)this, new JsonSerializerSettings
		{
			NullValueHandling = (NullValueHandling)1
		});
		return "jData=" + text;
	}
}
