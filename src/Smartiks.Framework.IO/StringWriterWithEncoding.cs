using System;
using System.IO;
using System.Text;

namespace Smartiks.Framework.IO
{
    public class StringWriterWithEncoding : StringWriter
    {
        public override Encoding Encoding { get; } = new UnicodeEncoding(false, false);

        public StringWriterWithEncoding()
        {
        }

        public StringWriterWithEncoding(Encoding encoding) : this()
        {
            Encoding = encoding;
        }

        public StringWriterWithEncoding(IFormatProvider formatProvider) : base(formatProvider)
        {
        }

        public StringWriterWithEncoding(IFormatProvider formatProvider, Encoding encoding) : this(formatProvider)
        {
            Encoding = encoding;
        }

        public StringWriterWithEncoding(StringBuilder sb) : base(sb)
        {
        }

        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding) : this(sb)
        {
            Encoding = encoding;
        }

        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider) : base(sb, formatProvider)
        {
        }

        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider, Encoding encoding) : this(sb, formatProvider)
        {
            Encoding = encoding;
        }
    }
}