using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Smartiks.Framework.IO.Xml.Abstractions;

namespace Smartiks.Framework.IO.Xml
{
    public class XmlDocumentService : IXmlDocumentService
    {
        private XmlSerializerFactory xmlSerializerFactory;

        public XmlDocumentService()
        {
            xmlSerializerFactory = new XmlSerializerFactory();
        }

        public Task<object> ReadAsync(XmlReader xmlReader, Type type)
        {
            if (xmlReader == null)
                throw new ArgumentNullException(nameof(xmlReader));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var xmlSerializer = xmlSerializerFactory.CreateSerializer(type);

            var result = xmlSerializer.Deserialize(xmlReader);

            return Task.FromResult(result);
        }

        public Task WriteAsync(XmlWriter xmlWriter, object document, Type type)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));

            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var xmlSerializer = xmlSerializerFactory.CreateSerializer(type);

            xmlSerializer.Serialize(xmlWriter, document);

            return Task.CompletedTask;
        }
    }
}
