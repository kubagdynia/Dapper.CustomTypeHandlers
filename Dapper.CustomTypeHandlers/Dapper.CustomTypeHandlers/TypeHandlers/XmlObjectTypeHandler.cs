using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Dapper.CustomTypeHandlers.Exceptions;
using Dapper.CustomTypeHandlers.Serializers;

namespace Dapper.CustomTypeHandlers.TypeHandlers
{
    public class XmlObjectTypeHandler : SqlMapper.ITypeHandler
    {
        private readonly XmlWriterSettings _xmlWriterSettings;

        public XmlObjectTypeHandler(XmlWriterSettings xmlWriterSettings)
        {
            _xmlWriterSettings = xmlWriterSettings;
        }

        public object Parse(Type destinationType, object value)
        {
            if (!typeof(IXmlObjectType).IsAssignableFrom(destinationType))
            {
                throw new DapperParseXmlObjectException(destinationType);
            }

            if (value == null || value is DBNull)
            {
                return null;
            }

            try
            {
                var result = DeserializeXml(destinationType, value);
                return result;
            }
            catch (Exception e)
            {
                throw new DapperParseXmlObjectException(value, e);
            }
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value == null || value is DBNull ? DBNull.Value : SerializeToXml(value);
        }

        private object SerializeToXml(object value)
        {
            var serializer = new XmlSerializer(value.GetType());

            using (var stream = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stream, _xmlWriterSettings))
                {
                    serializer.Serialize(writer, value, BaseXmlOptions.WithoutNamespaces);
                    var result = stream.ToString();
                    return result;
                }
            }
        }

        private object DeserializeXml(Type destinationType, object value)
        {
            var serializer = new XmlSerializer(destinationType);
            using (var stringReader = new StringReader((string)value))
            {
                return serializer.Deserialize(stringReader);
            }
        }
    }
}
