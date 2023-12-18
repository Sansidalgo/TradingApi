using System.Collections.Generic;
using System.Data;
using System.Reflection;
using NorenRestApiWrapper;

namespace NorenApiWrapper;

public static class NorenApiHelpers
{
	public static DataTable GetHoldingsDataTable(List<HoldingsItem> list)
	{
		DataTable dataTable = new DataTable(typeof(HoldingsItem).Name);
		FieldInfo[] fields = typeof(HoldingsItem).GetFields();
		typeof(ScripItem).GetFields();
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (!(fieldInfo.Name == "exch_tsym"))
			{
				dataTable.Columns.Add(fieldInfo.Name);
			}
		}
		dataTable.Columns.Add("exch_nse");
		dataTable.Columns.Add("ls_nse");
		dataTable.Columns.Add("pp_nse");
		dataTable.Columns.Add("ti_nse");
		dataTable.Columns.Add("token_nse");
		dataTable.Columns.Add("tsym_nse");
		dataTable.Columns.Add("exch_bse");
		dataTable.Columns.Add("ls_bse");
		dataTable.Columns.Add("pp_bse");
		dataTable.Columns.Add("ti_bse");
		dataTable.Columns.Add("token_bse");
		dataTable.Columns.Add("tsym_bse");
		foreach (HoldingsItem item in list)
		{
			object[] array2 = new object[dataTable.Columns.Count];
			int num = 0;
			for (int j = 0; j < fields.Length; j++)
			{
				if (!(fields[j].Name == "exch_tsym"))
				{
					array2[num++] = fields[j].GetValue(item);
				}
			}
			bool flag = false;
			foreach (ScripItem item2 in item.exch_tsym)
			{
				if (!(item2.exch != "NSE"))
				{
					array2[num++] = item2.exch;
					array2[num++] = item2.ls;
					array2[num++] = item2.pp;
					array2[num++] = item2.ti;
					array2[num++] = item2.token;
					array2[num++] = item2.tsym;
					flag = true;
				}
			}
			if (!flag)
			{
				num += 6;
			}
			foreach (ScripItem item3 in item.exch_tsym)
			{
				if (!(item3.exch != "BSE"))
				{
					array2[num++] = item3.exch;
					array2[num++] = item3.ls;
					array2[num++] = item3.pp;
					array2[num++] = item3.ti;
					array2[num++] = item3.token;
					array2[num++] = item3.tsym;
				}
			}
			dataTable.Rows.Add(array2);
		}
		return dataTable;
	}
}
