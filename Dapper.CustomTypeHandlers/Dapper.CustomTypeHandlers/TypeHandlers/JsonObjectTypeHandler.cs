using System;
using System.Data;
using System.Text.Json;

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
                throw new ArgumentException(
                    $"'{destinationType}' should implement '{nameof(IJsonObjectType)}' interface.", nameof(destinationType));
            }

            return JsonSerializer.Deserialize(value.ToString(), destinationType, _options);
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value == null || value is DBNull ? (object)DBNull.Value : JsonSerializer.Serialize(value, _options);
            parameter.DbType = DbType.String;
        }
    }
}
