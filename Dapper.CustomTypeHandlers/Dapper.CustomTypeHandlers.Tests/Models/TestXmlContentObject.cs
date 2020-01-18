using Dapper.CustomTypeHandlers.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.CustomTypeHandlers.Tests.Models
{
    public class TestXmlContentObject : IXmlObjectType
    {
        public int Siblings { get; set; }
        public string Nick { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<string> FavoriteDaysOfTheWeek { get; set; }
        public List<int> FavoriteNumbers { get; set; }
    }
}
