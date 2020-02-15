using System.Text.Json;
using System.Xml;

namespace Dapper.CustomTypeHandlers.Extensions
{
    public class DapperCustomTypeHandlersOptions
    {
        public bool RegisterXmlObjectTypeHandler { get; set; } = true;
        
        public bool RegisterJsonObjectTypeHandler { get; set; } = true;
        
        public bool RegisterGuidTypeHandler { get; set; } = true;
        
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        
        public XmlWriterSettings XmlWriterSettings { get; set; }
    }
}