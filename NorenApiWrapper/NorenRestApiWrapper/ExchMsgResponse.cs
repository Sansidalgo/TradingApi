using System.Collections.Generic;

namespace NorenRestApiWrapper;

public class ExchMsgResponse : NorenListResponseMsg<ExchMsgItem>
{
	public List<ExchMsgItem> messages => list;
}
