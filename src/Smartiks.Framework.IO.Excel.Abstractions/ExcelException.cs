using System;

namespace Smartiks.Framework.IO.Excel.Abstractions
{
    public class ExcelException : Exception
    {
        public ExcelException()
        {
        }

        public ExcelException(string message) : base(message)
        {
        }

        public ExcelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
