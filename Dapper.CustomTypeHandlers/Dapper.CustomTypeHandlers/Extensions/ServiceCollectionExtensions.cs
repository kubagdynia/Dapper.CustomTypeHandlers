using Dapper.CustomTypeHandlers.Serializers;
using Dapper.CustomTypeHandlers.TypeHandlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Xml;

namespace Dapper.CustomTypeHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="options"></param>
    public static IServiceCollection RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly assembly,
        Action<DapperCustomTypeHandlersOptions> options = null)
    {
        RegisterHandlers(new[] {assembly}, options);
        return services;
    }
        
    /// <summary>
    /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <param name="options"></param>
    public static IServiceCollection RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly[] assemblies,
        Action<DapperCustomTypeHandlersOptions> options = null)
    {
        RegisterHandlers(assemblies, options);
        return services;
    }

    /// <summary>
    /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
    /// </summary>
    /// <param name="assemblies"></param>
    /// <param name="options"></param>
    private static void RegisterHandlers(Assembly[] assemblies, Action<DapperCustomTypeHandlersOptions> options = null)
    {
        var opt = new DapperCustomTypeHandlersOptions();
        options?.Invoke(opt);
            
        // Xml
        if (opt.RegisterXmlObjectTypeHandler)
        {
            var xmlTypesFromAssemblies =
                assemblies.SelectMany(a =>
                    a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IXmlObjectType))));

            var xmlSettings = CreateXmlWriterSettings(opt.XmlWriterSettings);
            foreach (var type in xmlTypesFromAssemblies)
            {
                SqlMapper.AddTypeHandler(type, new XmlObjectTypeHandler(xmlSettings));
            }
        }

        // Json
        if (opt.RegisterJsonObjectTypeHandler)
        {
            var jsonTypesFromAssemblies =
                assemblies.SelectMany(a =>
                    a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IJsonObjectType))));

            var jsonOptions = CreateJsonSerializerOptions(opt.JsonSerializerOptions);
            foreach (var type in jsonTypesFromAssemblies)
            {
                SqlMapper.AddTypeHandler(type, new JsonObjectTypeHandler(jsonOptions));
            }
        }

        // Guid
        if (opt.RegisterGuidTypeHandler)
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
        }
    }

    /// <summary>
    /// Clear the registered type handlers.
    /// </summary>
    public static IServiceCollection ResetDapperCustomTypeHandlers(this IServiceCollection services)
    {
        SqlMapper.ResetTypeHandlers();
        return services;
    }

    private static XmlWriterSettings CreateXmlWriterSettings(XmlWriterSettings xmlWriterSettings = null)
        => xmlWriterSettings ?? BaseXmlOptions.GetXmlWriterSettings;

    private static JsonSerializerOptions CreateJsonSerializerOptions(JsonSerializerOptions jsonOptions = null)
        => jsonOptions ?? BaseJsonOptions.GetJsonSerializerOptions;
}