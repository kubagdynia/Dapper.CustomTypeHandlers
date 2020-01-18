using Dapper.CustomTypeHandlers.TypeHandlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Dapper.CustomTypeHandlers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDapperCustomTypeHandlers(this IServiceCollection services, Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            // Xml
            var xmlTypesFromAssemblies =
                assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IXmlObjectType))));

            foreach (var type in xmlTypesFromAssemblies)
            {
                SqlMapper.AddTypeHandler(type, new XmlObjectTypeHandler());
            }

            // Json
            var jsonTypesFromAssemblies =
                assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IJsonObjectType))));

            foreach (var type in jsonTypesFromAssemblies)
            {
                SqlMapper.AddTypeHandler(type, new JsonObjectTypeHandler());
            }
        }
    }
}
