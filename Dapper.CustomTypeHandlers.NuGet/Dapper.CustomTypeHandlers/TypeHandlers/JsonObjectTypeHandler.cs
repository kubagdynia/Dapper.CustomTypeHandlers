﻿using Dapper.CustomTypeHandlers.Serializers;
using System;
using System.Data;
using System.Text.Json;

namespace Dapper.CustomTypeHandlers.TypeHandlers
{
    public class JsonObjectTypeHandler : SqlMapper.ITypeHandler
    {
        public object Parse(Type destinationType, object value)
        {
            if (!typeof(IJsonObjectType).IsAssignableFrom(destinationType))
            {
                throw new ArgumentException(
                    $"'{destinationType}' should implement '{nameof(IJsonObjectType)}' interface.", nameof(destinationType));
            }

            return JsonSerializer.Deserialize(value.ToString(), destinationType, BaseJsonOptions.GetJsonSerializerOptions);
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value == null || value is DBNull ? (object)DBNull.Value : JsonSerializer.Serialize(value, BaseJsonOptions.GetJsonSerializerOptions);
            parameter.DbType = DbType.String;
        }
    }
}
