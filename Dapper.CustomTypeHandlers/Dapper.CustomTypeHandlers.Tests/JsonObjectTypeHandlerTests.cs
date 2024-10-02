using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using FluentAssertions;
using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Tests.Models;
using System.Collections.Generic;
using Dapper.CustomTypeHandlers.Tests.Helpers;
using Dapper.CustomTypeHandlers.Extensions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dapper.CustomTypeHandlers.Tests;

public class JsonObjectTypeHandlerTests
{
    [Test]
    public async Task Json_Data_Saved_In_DataBase_Should_Be_Properly_Restored()
    {
        ServiceCollection services =
            new ServiceCollectionBuilder().PrepareServiceCollection(s =>
            {
                s.ResetDapperCustomTypeHandlers();
                s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly());
            });

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using IServiceScope scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;

        ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

        TestJsonObject testObject = new TestJsonObject
        {
            FirstName = "John",
            LastName = "Doe",
            StartWork = new DateTime(2018, 06, 01),
            Content = new TestJsonContentObject
            {
                Nick = "JD",
                DateOfBirth = new DateTime(1990, 10, 11),
                Siblings = 2,
                FavoriteDaysOfTheWeek = new List<string>
                {
                    "Friday",
                    "Saturday",
                    "Sunday"
                },
                FavoriteNumbers = new List<int> { 10, 15, 1332, 5555 }
            }
        };

        // Act
        await testObjectRepository.SaveTestJsonObject(testObject);
        TestJsonObject retrievedTestObject = await testObjectRepository.GetTestJsonObject(testObject.Id);

        // Assert
        retrievedTestObject.Should().NotBeNull();
        retrievedTestObject.Should().BeEquivalentTo(testObject);
        retrievedTestObject.Content.Should().BeEquivalentTo(testObject.Content);
    }
        
    [Test]
    public async Task Using_JsonCustomOptions_Json_Data_Saved_In_DataBase_Should_Be_Properly_Restored()
    {
        ServiceCollection services =
            new ServiceCollectionBuilder().PrepareServiceCollection(s =>
            {
                s.ResetDapperCustomTypeHandlers();
                s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly(), options =>
                {
                    options.JsonSerializerOptions = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNamingPolicy = null
                    };
                });
            });

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using IServiceScope scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;

        ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

        TestJsonObject testObject = new TestJsonObject
        {
            FirstName = "John",
            LastName = "Doe",
            StartWork = new DateTime(2018, 06, 01),
            Content = new TestJsonContentObject
            {
                Nick = "JD",
                DateOfBirth = new DateTime(1990, 10, 11),
                Siblings = 2,
                FavoriteDaysOfTheWeek = new List<string>
                {
                    "Friday",
                    "Saturday",
                    "Sunday"
                },
                FavoriteNumbers = new List<int> { 10, 15, 1332, 5555 }
            }
        };

        // Act
        await testObjectRepository.SaveTestJsonObject(testObject);
        TestJsonObject retrievedTestObject = await testObjectRepository.GetTestJsonObject(testObject.Id);

        // Assert
        retrievedTestObject.Should().NotBeNull();
        retrievedTestObject.Should().BeEquivalentTo(testObject);
        retrievedTestObject.Content.Should().BeEquivalentTo(testObject.Content);
    }

    [Test]
    public async Task Null_Json_Data_Saved_In_DataBase_Should_Be_Restored_As_Null_Object()
    {
        ServiceCollection services =
            new ServiceCollectionBuilder().PrepareServiceCollection(s =>
            {
                s.ResetDapperCustomTypeHandlers();
                s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly());
            });

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using IServiceScope scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;

        ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

        TestJsonObject testObject = new TestJsonObject
        {
            FirstName = "John",
            LastName = "Doe",
            StartWork = new DateTime(2018, 06, 01),
            Content = null
        };

        // Act
        await testObjectRepository.SaveTestJsonObject(testObject);
        TestJsonObject retrievedTestObject = await testObjectRepository.GetTestJsonObject(testObject.Id);

        // Assert
        retrievedTestObject.Should().NotBeNull();
        retrievedTestObject.Should().BeEquivalentTo(testObject);
        retrievedTestObject.Content.Should().BeEquivalentTo(testObject.Content);
    }
}