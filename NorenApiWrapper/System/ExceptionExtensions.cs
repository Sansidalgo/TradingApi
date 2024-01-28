using System.Collections.Generic;
using System.Linq;

namespace System;

internal static class ExceptionExtensions
{
	public static IEnumerable<string> Messages(this Exception ex)
	{
		if (ex == null)
		{
			yield break;
		}
		yield return ex.Message;
		IEnumerable<Exception> enumerable = Enumerable.Empty<Exception>();
		if (ex is AggregateException && (ex as AggregateException).InnerExceptions.Any())
		{
			enumerable = (ex as AggregateException).InnerExceptions;
		}
		else if (ex.InnerException != null)
		{
			enumerable = new Exception[1] { ex.InnerException };
		}
		foreach (Exception item in enumerable)
		{
			foreach (string item2 in item.Messages())
			{
				yield return item2;
			}
		}
	}
}
