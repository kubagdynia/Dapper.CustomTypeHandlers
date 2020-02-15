using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Tests.Models;

namespace Dapper.CustomTypeHandlers.Tests.Repositories
{
    internal interface ITestObjectRepository
    {
        Task<TestXmlObject> GetTestXmlObject(long id);
        Task SaveTestXmlObject(TestXmlObject testObject);
        Task<TestJsonObject> GetTestJsonObject(long id);
        Task SaveTestJsonObject(TestJsonObject testObject);
    }
}