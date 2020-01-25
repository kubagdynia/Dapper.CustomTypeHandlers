using Dapper.CustomTypeHandlers.Serializers;
using Dapper.CustomTypeHandlers.TypeHandlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Xml;

namespace Dapper.CustomTypeHandlers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
        /// </summary>
        public static void RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly assembly,
            Action<JsonSerializerOptions> jsonSerializerOptions)
            => RegisterDapperCustomTypeHandlers(services, assembly, ServiceLifetime.Transient, jsonSerializerOptions);

        /// <summary>
        /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
        /// </summary>
        public static void RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly assembly,
            Action<XmlWriterSettings> xmlWriterSettings)
            => RegisterDapperCustomTypeHandlers(services, assembly, ServiceLifetime.Transient, null, xmlWriterSettings);

        /// <summary>
        /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
        /// </summary>
        public static void RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly assembly,
            Action<JsonSerializerOptions> jsonSerializerOptions, Action<XmlWriterSettings> xmlWriterSettings)
            => RegisterDapperCustomTypeHandlers(services, assembly, ServiceLifetime.Transient, jsonSerializerOptions,
                xmlWriterSettings);

        /// <summary>
        /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
        /// </summary>
        public static void RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Action<JsonSerializerOptions> jsonSerializerOptions = null,
            Action<XmlWriterSettings> xmlWriterSettings = null)
            => RegisterDapperCustomTypeHandlers(services, new[] {assembly}, lifetime, jsonSerializerOptions,
                xmlWriterSettings);

        /// <summary>
        /// Register all specified type to be processed by a custom Dapper Xml and Json handlers 
        /// </summary>
        public static void RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Action<JsonSerializerOptions> jsonSerializerOptions = null,
            Action<XmlWriterSettings> xmlWriterSettings = null)
        {
            // Xml
            var xmlTypesFromAssemblies =
                assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IXmlObjectType))));

            var xmlSettings = CreateXmlWriterSettings(xmlWriterSettings);
            foreach (var type in xmlTypesFromAssemblies)
            {
                SqlMapper.AddTypeHandler(type, new XmlObjectTypeHandler(xmlSettings));
            }

            // Json
            var jsonTypesFromAssemblies =
                assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IJsonObjectType))));
            
            var jsonOptions = CreateJsonSerializerOptions(jsonSerializerOptions);
            foreach (var type in jsonTypesFromAssemblies)
            {
                SqlMapper.AddTypeHandler(type, new JsonObjectTypeHandler(jsonOptions));
            }
        }

        /// <summary>
        /// Clear the registered type handlers.
        /// </summary>
        public static void ResetDapperCustomTypeHandlers(this IServiceCollection services)
            => SqlMapper.ResetTypeHandlers();

        private static XmlWriterSettings CreateXmlWriterSettings(Action<XmlWriterSettings> xmlWriterSettings)
        {
            var settings = new XmlWriterSettings();
            if (xmlWriterSettings == null)
            {
                settings = BaseXmlOptions.GetXmlWriterSettings;
            }
            else
            {
                xmlWriterSettings(settings);
            }

            return settings;
        }

        private static JsonSerializerOptions CreateJsonSerializerOptions(Action<JsonSerializerOptions> jsonOptions)
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            if (jsonOptions == null)
            {
                jsonSerializerOptions = BaseJsonOptions.GetJsonSerializerOptions;
            }
            else
            {
                jsonOptions(jsonSerializerOptions);
            }

            return jsonSerializerOptions;
        }
    }
}
