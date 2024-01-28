using System;
using System.Runtime.Serialization;

namespace NorenRestApiWrapper;

[Serializable]
public class UndefinedVariableException : Exception
{
	public UndefinedVariableException()
	{
	}

	public UndefinedVariableException(string message)
		: base(message)
	{
	}

	public UndefinedVariableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UndefinedVariableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
