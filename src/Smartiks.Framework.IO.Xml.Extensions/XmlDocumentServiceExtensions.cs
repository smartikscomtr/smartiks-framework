using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Smartiks.Framework.IO.Xml.Abstractions;

namespace Smartiks.Framework.IO.Xml.Extensions
{
    public static class XmlDocumentServiceExtensions
    {
        public static async Task<object> ReadAsync(this IXmlDocumentService xmlDocumentService, string xml, Type type)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    return await xmlDocumentService.ReadAsync(xmlReader, type);
                }
            }
        }

        public static async Task<object> ReadAsync(this IXmlDocumentService xmlDocumentService, StreamReader streamReader, Type type)
        {
            if (streamReader == null)
                throw new ArgumentNullException(nameof(streamReader));

            using (var xmlReader = XmlReader.Create(streamReader))
            {
                return await xmlDocumentService.ReadAsync(xmlReader, type);
            }
        }

        public static async Task<object> ReadAsync(this IXmlDocumentService xmlDocumentService, Stream stream, Encoding encoding, Type type)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            using (var streamReader = new StreamReader(stream, encoding, true, 1024, true))
            {
                return await xmlDocumentService.ReadAsync(streamReader, type);
            }
        }

        public static async Task<object> ReadAsync(this IXmlDocumentService xmlDocumentService, string filePath, Encoding encoding, Type type)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return await xmlDocumentService.ReadAsync(stream, encoding, type);
            }
        }

        public static async Task<T> ReadAsync<T>(this IXmlDocumentService xmlDocumentService, XmlReader xmlReader)
        {
            return (T) await xmlDocumentService.ReadAsync(xmlReader, typeof(T));
        }

        public static async Task<T> ReadAsync<T>(this IXmlDocumentService xmlDocumentService, string xml)
        {
            return (T) await xmlDocumentService.ReadAsync(xml, typeof(T));
        }

        public static async Task<T> ReadAsync<T>(this IXmlDocumentService xmlDocumentService, StreamReader streamReader)
        {
            return (T) await xmlDocumentService.ReadAsync(streamReader, typeof(T));
        }

        public static async Task<T> ReadAsync<T>(this IXmlDocumentService xmlDocumentService, Stream stream, Encoding encoding)
        {
            return (T) await xmlDocumentService.ReadAsync(stream, encoding, typeof(T));
        }

        public static async Task<T> ReadAsync<T>(this IXmlDocumentService xmlDocumentService, string filePath, Encoding encoding)
        {
            return (T) await xmlDocumentService.ReadAsync(filePath, encoding, typeof(T));
        }

        public static async Task<string> WriteAsync(this IXmlDocumentService xmlDocumentService, Encoding encoding, object document, Type type)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            using (var stringWriter = new StringWriter(encoding))
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    await xmlDocumentService.WriteAsync(xmlWriter, document, type);
                }

                return stringWriter.ToString();
            }
        }

        public static async Task WriteAsync(this IXmlDocumentService xmlDocumentService, StreamWriter streamWriter, object document, Type type)
        {
            if (streamWriter == null)
                throw new ArgumentNullException(nameof(streamWriter));

            using (var xmlWriter = XmlWriter.Create(streamWriter))
            {
                await xmlDocumentService.WriteAsync(xmlWriter, document, type);
            }
        }

        public static async Task WriteAsync(this IXmlDocumentService xmlDocumentService, Stream stream, Encoding encoding, object document, Type type)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            using (var streamWriter = new StreamWriter(stream, encoding, 1024, true))
            {
                await xmlDocumentService.WriteAsync(streamWriter, document, type);
            }
        }

        public static async Task WriteAsync(this IXmlDocumentService xmlDocumentService, string filePath, Encoding encoding, object document, Type type)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await xmlDocumentService.WriteAsync(stream, encoding, document, type);
            }
        }

        public static async Task WriteAsync<T>(this IXmlDocumentService xmlDocumentService, XmlWriter xmlWriter, T document)
        {
            await xmlDocumentService.WriteAsync(xmlWriter, document, typeof(T));
        }

        public static async Task<string> WriteAsync<T>(this IXmlDocumentService xmlDocumentService, Encoding encoding, T document)
        {
            return await xmlDocumentService.WriteAsync(encoding, document, typeof(T));
        }

        public static async Task WriteAsync<T>(this IXmlDocumentService xmlDocumentService, StreamWriter streamWriter, T document)
        {
            await xmlDocumentService.WriteAsync(streamWriter, document, typeof(T));
        }

        public static async Task WriteAsync<T>(this IXmlDocumentService xmlDocumentService, Stream stream, Encoding encoding, T document)
        {
            await xmlDocumentService.WriteAsync(stream, encoding, document, typeof(T));
        }

        public static async Task WriteAsync<T>(this IXmlDocumentService xmlDocumentService, string filePath, Encoding encoding, T document)
        {
            await xmlDocumentService.WriteAsync(filePath, encoding, document, typeof(T));
        }
    }
}
