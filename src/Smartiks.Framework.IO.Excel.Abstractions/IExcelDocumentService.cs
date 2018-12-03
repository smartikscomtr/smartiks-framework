using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Smartiks.Framework.IO.Excel.Abstractions
{
    public interface IExcelDocumentService
    {
        IReadOnlyCollection<string> GetWorksheetNames(Stream excelStream);

        IReadOnlyCollection<object> Read(Stream excelStream, string worksheetName, Type type, CultureInfo cultureInfo);

        void Write(Stream excelStream, string worksheetName, Type type, IEnumerable items, CultureInfo cultureInfo);
    }
}