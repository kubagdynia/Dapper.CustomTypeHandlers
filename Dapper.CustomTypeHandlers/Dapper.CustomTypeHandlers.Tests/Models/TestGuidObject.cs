using System;

namespace Dapper.CustomTypeHandlers.Tests.Models
{
    public class TestGuidObject
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
    }
}