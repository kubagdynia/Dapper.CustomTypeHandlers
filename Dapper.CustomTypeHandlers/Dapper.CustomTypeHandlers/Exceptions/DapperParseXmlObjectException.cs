using System;
using Dapper.CustomTypeHandlers.TypeHandlers;

namespace Dapper.CustomTypeHandlers.Exceptions
{
    public class DapperParseXmlObjectException : DapperCustomTypeHandlersException
    {
        public Type DestinationType { get; }
        public object ParseObject { get; }

        public DapperParseXmlObjectException(Type destinationType)
            : base($"'{destinationType}' should implement '{nameof(IXmlObjectType)}' interface marker.")
        {
            DestinationType = destinationType;
        }
        
        public DapperParseXmlObjectException(object parseObject, Exception e)
            : base($"Object with value: {parseObject} is not a valid XML. {e.Message}")
        {
            ParseObject = parseObject;
        }
    }
}