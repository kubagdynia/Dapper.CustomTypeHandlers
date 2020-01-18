using Dapper.CustomTypeHandlers.Tests.DbConnection;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Dapper.CustomTypeHandlers.Extensions;

namespace Dapper.CustomTypeHandlers.Tests.Helpers
{
    internal static class ServiceCollectionBuilder
    {
        public static ServiceCollection PrepareServiceCollection()
        {
            ServiceCollection services = new ServiceCollection();

            // Search the specified assembly and register all classes that implement IXmlObjectType and IJsonObjectType interfaces
            services.RegisterDapperCustomTypeHandlers(new[] { Assembly.GetExecutingAssembly() });

            services.AddTransient<IDbConnectionFactory, SqliteConnectionFactory>();
            services.AddTransient<ITestObjectRepository, TestObjectRepository>();

            return services;
        }
    }
}
