using Dapper.CustomTypeHandlers.Tests.DbConnection;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dapper.CustomTypeHandlers.Tests.Helpers;

internal class ServiceCollectionBuilder
{
    public ServiceCollection PrepareServiceCollection(Action<ServiceCollection> serviceCollection = null)
    {
        var services = new ServiceCollection();

        serviceCollection?.Invoke(services);

        services.AddTransient<IDbConnectionFactory, SqliteConnectionFactory>();
        services.AddTransient<ITestObjectRepository, TestObjectRepository>();

        return services;
    }
        
    public ServiceCollection PrepareServiceCollectionForGuidTests(Action<ServiceCollection> serviceCollection = null)
    {
        var services = new ServiceCollection();

        serviceCollection?.Invoke(services);

        services.AddTransient<IDbConnectionFactory, SqliteConnectionFactoryForGuid>();
        services.AddTransient<ITestGuidRepository, TestGuidRepository>();

        return services;
    }
}