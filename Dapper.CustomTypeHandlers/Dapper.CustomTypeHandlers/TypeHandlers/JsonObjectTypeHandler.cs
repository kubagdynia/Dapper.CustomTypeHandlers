using System;
using System.Data;
using System.Text.Json;
using Dapper.CustomTypeHandlers.Exceptions;

namespace Dapper.CustomTypeHandlers.TypeHandlers
{
    public class JsonObjectTypeHandler : SqlMapper.ITypeHandler
    {
        private readonly JsonSerializerOptions _options;

        public JsonObjectTypeHandler(JsonSerializerOptions options)
        {
            _options = options;
        }

        public object Parse(Type destinationType, object value)
        {
            if (!typeof(IJsonObjectType).IsAssignableFrom(destinationType))
            {
                throw new DapperParseJsonObjectException(destinationType);
            }

            try
            {
                return JsonSerializer.Deserialize(value.ToString(), destinationType, _options);
            }
            catch (Exception e)
            {
                throw new DapperParseJsonObjectException(value, e);
            }
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value is null or DBNull ? DBNull.Value : JsonSerializer.Serialize(value, _options);
            parameter.DbType = DbType.String;
        }
    }
}
