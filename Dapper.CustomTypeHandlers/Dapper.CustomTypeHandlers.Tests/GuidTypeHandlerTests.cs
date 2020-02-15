using System;
using System.Reflection;
using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Extensions;
using Dapper.CustomTypeHandlers.Tests.Helpers;
using Dapper.CustomTypeHandlers.Tests.Models;
using Dapper.CustomTypeHandlers.Tests.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dapper.CustomTypeHandlers.Tests
{
    public class GuidTypeHandlerTests
    {
        [Test, Order(1)]
        public void Always_Success_Test_Should_Be_Ok()
        {
            1.Should().Equals(1);
        }

        [Test, Order(2)]
        public async Task Guid_Data_Saved_In_DataBase_Should_Be_Properly_Restored()
        {
            ServiceCollection services =
                new ServiceCollectionBuilder().PrepareServiceCollectionForGuidTests(s =>
                {
                    s.ResetDapperCustomTypeHandlers();
                    s.RegisterDapperCustomTypeHandlers(Assembly.GetExecutingAssembly());
                });

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                
                ITestGuidRepository testGuidRepository = scopedServices.GetRequiredService<ITestGuidRepository>();
                
                TestGuidObject testGuidObject = new TestGuidObject
                {
                    GuidId = Guid.NewGuid()
                };
                
                // Act
                await testGuidRepository.SaveTestGuidObject(testGuidObject);
                TestGuidObject retrievedTestGuidObject = await testGuidRepository.GetTestGuidObject(testGuidObject.Id);
                
                // Assert
                retrievedTestGuidObject.Should().NotBeNull();
                retrievedTestGuidObject.Should().BeEquivalentTo(testGuidObject);
                retrievedTestGuidObject.GuidId.Should().Be(testGuidObject.GuidId);
            }
        }
    }
}