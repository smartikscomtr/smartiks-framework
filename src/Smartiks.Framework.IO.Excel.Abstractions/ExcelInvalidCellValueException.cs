using System;

namespace Smartiks.Framework.IO.Excel.Abstractions
{
    public class ExcelInvalidCellValueException : ExcelException
    {
        public int RowNo { get; }

        public int ColumnNo { get; }

        public string Address { get; }

        public string Property { get; }

        public ExcelInvalidCellValueException(int rowNo, int columnNo, string address, string property, Exception innerException) : base(string.Empty, innerException)
        {
            RowNo = rowNo;

            ColumnNo = columnNo;

            Address = address;

            Property = property;
        }
    }
}
