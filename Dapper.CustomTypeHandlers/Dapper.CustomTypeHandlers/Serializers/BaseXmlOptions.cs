using System.Xml;
using System.Xml.Serialization;

namespace Dapper.CustomTypeHandlers.Serializers
{
    public static class BaseXmlOptions
    {
        public static bool Indent { get; } = true;
        
        public static bool OmitXmlDeclaration { get; } = true;
        
        public static XmlWriterSettings GetXmlWriterSettings { get; } = new XmlWriterSettings
        {
            Indent = Indent,
            OmitXmlDeclaration = OmitXmlDeclaration
        };

        public static readonly XmlSerializerNamespaces WithoutNamespaces =
            new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
    }
}