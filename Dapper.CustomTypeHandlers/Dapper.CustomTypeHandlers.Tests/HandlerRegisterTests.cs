using System;
using System.Reflection;
using Dapper.CustomTypeHandlers.Extensions;
using Dapper.CustomTypeHandlers.Tests.Helpers;
using Dapper.CustomTypeHandlers.Tests.Models;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dapper.CustomTypeHandlers.Tests
{
    public class HandlerRegisterTests
    {
        [Test, Order(1)]
        public void Always_Success_Test_Should_Be_Ok()
        {
            1.Should().Equals(1);
        }

        [Test, Order(2)]
        public void When_Custom_Json_Handler_Is_Not_Registered_Exception_Should_Be_Thrown()
        {
            ServiceCollection services =
                new ServiceCollectionBuilder().PrepareServiceCollection(s =>
                {
                    s.ResetDapperCustomTypeHandlers();
                });
            
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

                TestJsonObject testObject = CreateTestJsonObject();

                // Assert
                Assert.ThrowsAsync<NotSupportedException>(async () => await testObjectRepository.SaveTestJsonObject(testObject));
            }
        }
        
        [Test, Order(3)]
        public void When_Custom_Json_Handler_Is_Registered_Exception_Should_Not_Be_Thrown()
        {
            ServiceCollection services =
                new ServiceCollectionBuilder().PrepareServiceCollection(s =>
                {
                    s.ResetDapperCustomTypeHandlers();
                    s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly());
                });
            
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

                TestJsonObject testObject = CreateTestJsonObject();

                // Assert
                Assert.DoesNotThrowAsync(async () => await testObjectRepository.SaveTestJsonObject(testObject));
            }
        }
        
        [Test, Order(4)]
        public void When_Custom_Xml_Handler_Is_Not_Registered_Exception_Should_Be_Thrown()
        {
            ServiceCollection services =
                new ServiceCollectionBuilder().PrepareServiceCollection(s =>
                {
                    s.ResetDapperCustomTypeHandlers();
                });
            
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

                TestXmlObject testObject  = CreateTestXmlObject();

                // Assert
                Assert.ThrowsAsync<NotSupportedException>(async () => await testObjectRepository.SaveTestXmlObject(testObject));
            }
        }
        
        [Test, Order(5)]
        public void When_Custom_Xml_Handler_Is_Registered_Exception_Should_Not_Be_Thrown()
        {
            ServiceCollection services =
                new ServiceCollectionBuilder().PrepareServiceCollection(s =>
                {
                    s.ResetDapperCustomTypeHandlers();
                    s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly());
                });
            
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                ITestObjectRepository testObjectRepository = scopedServices.GetRequiredService<ITestObjectRepository>();

                TestXmlObject testObject  = CreateTestXmlObject();

                // Assert
                Assert.DoesNotThrowAsync(async () => await testObjectRepository.SaveTestXmlObject(testObject));
            }
        }

        private TestJsonObject CreateTestJsonObject()
            => new TestJsonObject
            {
                FirstName = "John",
                LastName = "Doe",
                StartWork = new DateTime(2018, 06, 01),
                Content = null
            };

        private TestXmlObject CreateTestXmlObject()
            => new TestXmlObject
            {
                FirstName = "John",
                LastName = "Doe",
                StartWork = new DateTime(2018, 06, 01),
                Content = null
            };
    }
}