using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Smartiks.Framework.IO.Excel.Abstractions;

namespace Smartiks.Framework.IO.Excel.Extensions
{
    public static class ExcelDocumentServiceExtensions
    {
        public static IReadOnlyCollection<T> Read<T>(this IExcelDocumentService excelDocumentService, Stream excelStream, string worksheetName, CultureInfo cultureInfo)
        {
            return
                excelDocumentService
                    .Read(excelStream, worksheetName, typeof(T), cultureInfo)
                    .Cast<T>()
                    .ToList()
                    .AsReadOnly();
        }

        public static void Write<T>(this IExcelDocumentService excelDocumentService, Stream excelStream, string worksheetName, IEnumerable items, IFormatProvider formatProvider)
        {
            excelDocumentService
                .Write(excelStream, worksheetName, typeof(T), items, formatProvider);
        }
    }
}
