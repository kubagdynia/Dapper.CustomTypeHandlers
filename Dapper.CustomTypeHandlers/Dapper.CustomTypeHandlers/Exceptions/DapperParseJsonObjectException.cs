using System;
using Dapper.CustomTypeHandlers.TypeHandlers;

namespace Dapper.CustomTypeHandlers.Exceptions;

public class DapperParseJsonObjectException : DapperCustomTypeHandlersException
{
    public Type DestinationType { get; }
    public object ParseObject { get; }

    public DapperParseJsonObjectException(Type destinationType)
        : base($"'{destinationType}' should implement '{nameof(IJsonObjectType)}' interface marker.")
    {
        DestinationType = destinationType;
    }
        
    public DapperParseJsonObjectException(object parseObject, Exception e)
        : base($"Object with value: {parseObject} is not a valid JSON. {e.Message}")
    {
        ParseObject = parseObject;
    }
}