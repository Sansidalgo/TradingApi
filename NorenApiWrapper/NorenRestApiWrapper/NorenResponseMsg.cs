using System;
using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenResponseMsg
{
	public string stat;

	public string emsg;

	public virtual string toJson()
	{
		//IL_001c: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		string empty = string.Empty;
		try
		{
			return JsonConvert.SerializeObject((object)this, new JsonSerializerSettings
			{
				NullValueHandling = (NullValueHandling)1
			});
		}
		catch (JsonSerializationException val)
		{
			JsonSerializationException val2 = val;
			Console.WriteLine("Error deserializing data " + ((object)val2).ToString());
			return null;
		}
	}
}
