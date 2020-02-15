using System;
using System.Data;
using Dapper.CustomTypeHandlers.Exceptions;

namespace Dapper.CustomTypeHandlers.TypeHandlers
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value)
        {
            if (!Guid.TryParse((string)value, out Guid result))
            {
                throw new DapperParseGuidException(value);
            }

            return result;
        }
    }
}