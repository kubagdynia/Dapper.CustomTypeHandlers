using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Tests.Models;

namespace Dapper.CustomTypeHandlers.Tests.Repositories
{
    internal interface ITestGuidRepository
    {
        Task<TestGuidObject> GetTestGuidObject(long id);
        Task SaveTestGuidObject(TestGuidObject testObject);
    }
}