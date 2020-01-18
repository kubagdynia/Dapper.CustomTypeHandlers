using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using FluentAssertions;
using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Tests.Models;
using System.Collections.Generic;
using Dapper.CustomTypeHandlers.Tests.Helpers;

namespace Dapper.CustomTypeHandlers.Tests
{
    public class JsonObjectTypeHandlerTests
    {
        [Test, Order(1)]
        public void Always_Success_Test_Should_Be_Ok()
        {
            1.Should().Equals(1);
        }


        [Test, Order(2)]
        public async Task Json_Data_Saved_In_DataBase_Should_Be_Properly_Restored()
        {
            ServiceCollection services = ServiceCollectionBuilder.PrepareServiceCollection();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
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
        }

        [Test, Order(3)]
        public async Task Null_Json_Data_Saved_In_DataBase_Should_Be_Restored_As_Null_Object()
        {
            ServiceCollection services = ServiceCollectionBuilder.PrepareServiceCollection();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
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
    }
}
