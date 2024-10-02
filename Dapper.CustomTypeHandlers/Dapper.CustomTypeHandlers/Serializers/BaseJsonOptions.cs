using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dapper.CustomTypeHandlers.Serializers;

public static class BaseJsonOptions
{
    public static bool IgnoreNullValues { get; } = true;
    public static JsonNamingPolicy PropertyNamingPolicy { get; } = JsonNamingPolicy.CamelCase;

    public static JsonSerializerOptions GetJsonSerializerOptions { get; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = PropertyNamingPolicy,
    };
}