namespace Dapper.CustomTypeHandlers.Exceptions;

public class DapperParseGuidException : DapperCustomTypeHandlersException
{
    public object ParseObject { get; }

    public DapperParseGuidException(object parseObject) 
        : base($"Object with value: {parseObject} is not a valid Guid")
    {
        ParseObject = parseObject;
    }
}