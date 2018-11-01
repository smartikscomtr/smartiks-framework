using System;

namespace Smartiks.Framework.IO.Excel.Abstractions
{
    public class ExcelWorksheetEmptyException : ExcelException
    {
        public ExcelWorksheetEmptyException()
        {
        }

        public ExcelWorksheetEmptyException(string message) : base(message)
        {
        }

        public ExcelWorksheetEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
