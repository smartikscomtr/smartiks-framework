using System;
using System.Text;

namespace Smartiks.Framework.IO
{
    public class StringWriter : System.IO.StringWriter
    {
        public StringWriter()
        {
            Encoding = new UnicodeEncoding(false, false);
        }

        public StringWriter(Encoding encoding)
        {
            Encoding = encoding;
        }

        public StringWriter(IFormatProvider formatProvider) : base(formatProvider)
        {
            Encoding = new UnicodeEncoding(false, false);
        }

        public StringWriter(Encoding encoding, IFormatProvider formatProvider) : base(formatProvider)
        {
            Encoding = encoding;
        }

        public StringWriter(StringBuilder sb) : base(sb)
        {
            Encoding = new UnicodeEncoding(false, false);
        }

        public StringWriter(StringBuilder sb, Encoding encoding) : base(sb)
        {
            Encoding = encoding;
        }

        public StringWriter(StringBuilder sb, IFormatProvider formatProvider) : base(sb, formatProvider)
        {
            Encoding = new UnicodeEncoding(false, false);
        }

        public StringWriter(StringBuilder sb, Encoding encoding, IFormatProvider formatProvider) : base(sb, formatProvider)
        {
            Encoding = encoding;
        }

        public override Encoding Encoding { get; }
    }
}