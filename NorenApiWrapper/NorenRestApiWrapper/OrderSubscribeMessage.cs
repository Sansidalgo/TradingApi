namespace NorenRestApiWrapper;

public class OrderSubscribeMessage : NorenStreamMessage
{
	public string actid;

	public OrderSubscribeMessage()
	{
		t = "o";
	}
}
