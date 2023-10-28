using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Reflection;
using Dapper.CustomTypeHandlers.Extensions;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using FluentAssertions;
using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Tests.Models;
using System.Collections.Generic;
using System.Xml;
using Dapper.CustomTypeHandlers.Tests.Helpers;

namespace Dapper.CustomTypeHandlers.Tests
{
    public class XmlObjectTypeHandlerTests
    {
        [Test]
        public async Task Xml_Data_Saved_In_DataBase_Should_Be_Properly_Restored()
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

            TestXmlObject testObject = CreateFullTestObject();

            // Act
            await testObjectRepository.SaveTestXmlObject(testObject);
            TestXmlObject retrievedTestObject = await testObjectRepository.GetTestXmlObject(testObject.Id);

            // Assert
            retrievedTestObject.Should().NotBeNull();
            retrievedTestObject.Should().BeEquivalentTo(testObject);
            retrievedTestObject.Content.Should().BeEquivalentTo(testObject.Content);
        }
        
        [Test]
        public async Task Using_XmlCustomSettings_Xml_Data_Saved_In_DataBase_Should_Be_Properly_Restored1()
        {
            ServiceCollection services =
                new ServiceCollectionBuilder().PrepareServiceCollection(s =>
                {
                    s.ResetDapperCustomTypeHandlers();
                    s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly(), options =>
                    {
                        options.XmlWriterSettings = new XmlWriterSettings
                        {
                            Indent = false,
                            OmitXmlDeclaration = false
                        };
                    });
                });

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;

            ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

            TestXmlObject testObject = CreateFullTestObject();

            // Act
            await testObjectRepository.SaveTestXmlObject(testObject);
            TestXmlObject retrievedTestObject = await testObjectRepository.GetTestXmlObject(testObject.Id);

            // Assert
            retrievedTestObject.Should().NotBeNull();
            retrievedTestObject.Should().BeEquivalentTo(testObject);
            retrievedTestObject.Content.Should().BeEquivalentTo(testObject.Content);
        }

        [Test]
        public async Task Null_Xml_Data_Saved_In_DataBase_Should_Be_Restored_As_Null_Object()
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

            TestXmlObject testObject = new TestXmlObject
            {
                FirstName = "John",
                LastName = "Doe",
                StartWork = new DateTime(2018, 06, 01),
                Content = null
            };

            // Act
            await testObjectRepository.SaveTestXmlObject(testObject);
            TestXmlObject retrievedTestObject = await testObjectRepository.GetTestXmlObject(testObject.Id);

            // Assert
            retrievedTestObject.Should().NotBeNull();
            retrievedTestObject.Should().BeEquivalentTo(testObject);
            retrievedTestObject.Content.Should().BeEquivalentTo(testObject.Content);
        }

        private TestXmlObject CreateFullTestObject()
            => new()
            {
                FirstName = "John",
                LastName = "Doe",
                StartWork = new DateTime(2018, 06, 01),
                Content = new TestXmlContentObject
                {
                    Nick = "JD",
                    DateOfBirth = new DateTime(1990, 10, 11),
                    Siblings = 2,
                    FavoriteDaysOfTheWeek = new List<string>
                    {
                        "Friday",
                        "Saturday"
                    },
                    FavoriteNumbers = new List<int> {-502, 444, 0, 777777}
                }
            };

    }
}
