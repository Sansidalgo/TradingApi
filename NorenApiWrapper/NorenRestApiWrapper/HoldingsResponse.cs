using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using NorenApiWrapper;

namespace NorenRestApiWrapper;

public class HoldingsResponse : NorenListResponseMsg<HoldingsItem>
{
	public List<HoldingsItem> holdings => list;

	[JsonIgnore]
	public new DataView dataView => NorenApiHelpers.GetHoldingsDataTable(list).DefaultView;
}
