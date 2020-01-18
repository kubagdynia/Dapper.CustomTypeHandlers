using System;

namespace Dapper.CustomTypeHandlers.Tests.Models
{
    public class TestJsonObject
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartWork { get; set; }
        public TestJsonContentObject Content { get; set; }
    }
}
