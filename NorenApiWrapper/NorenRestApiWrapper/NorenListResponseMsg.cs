using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;

namespace NorenRestApiWrapper;

public class NorenListResponseMsg<T> : StandardResponse
{
	public List<T> list;

	[JsonIgnore]
	public DataView dataView
	{
		get
		{
			DataTable dataTable = new DataTable(typeof(T).Name);
			FieldInfo[] fields = typeof(T).GetFields();
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				dataTable.Columns.Add(fieldInfo.Name);
			}
			foreach (T item in list)
			{
				object[] array2 = new object[fields.Length];
				for (int j = 0; j < fields.Length; j++)
				{
					array2[j] = fields[j].GetValue(item);
				}
				dataTable.Rows.Add(array2);
			}
			return dataTable.DefaultView;
		}
	}

	public NorenListResponseMsg()
	{
		list = new List<T>();
	}

	public void Copy(NorenResponseMsg baseObject)
	{
		stat = baseObject.stat;
		emsg = baseObject.emsg;
		list = null;
	}
}
