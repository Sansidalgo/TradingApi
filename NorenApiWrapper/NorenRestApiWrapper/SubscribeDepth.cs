namespace NorenRestApiWrapper;

public class SubscribeDepth : NorenStreamMessage
{
	public string k;

	public SubscribeDepth()
	{
		t = "d";
	}
}
