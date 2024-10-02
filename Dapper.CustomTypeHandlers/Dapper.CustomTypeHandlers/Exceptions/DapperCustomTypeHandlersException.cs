using System;

namespace Dapper.CustomTypeHandlers.Exceptions;

public abstract class DapperCustomTypeHandlersException : Exception
{
    protected DapperCustomTypeHandlersException(string message) : base(message)
    {
            
    }
}