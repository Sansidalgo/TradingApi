namespace NorenRestApiWrapper;

public class UnSubscribeDepth : NorenStreamMessage
{
	public string k;

	public UnSubscribeDepth()
	{
		t = "ud";
	}
}
