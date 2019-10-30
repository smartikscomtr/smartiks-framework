using System;
using System.Threading.Tasks;
using System.Xml;

namespace Smartiks.Framework.IO.Xml.Abstractions
{
    public interface IXmlDocumentService
    {
        Task<object> ReadAsync(XmlReader xmlReader, Type type);

        Task WriteAsync(XmlWriter xmlWriter, object document, Type type);
    }
}