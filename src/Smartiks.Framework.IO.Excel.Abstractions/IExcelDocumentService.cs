using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Smartiks.Framework.IO.Excel.Abstractions
{
    public interface IExcelDocumentService
    {
        Task<IList<string>> GetWorksheetNamesAsync(Stream stream);

        Task<IList> ReadAsync(Stream stream, string worksheetName, Type type, CultureInfo cultureInfo);

        Task WriteAsync(Stream stream, string worksheetName, IEnumerable items, Type type, CultureInfo cultureInfo);
    }
}