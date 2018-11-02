using Smartiks.Framework.IO.Excel.Abstractions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

        public static void Write<T>(this IExcelDocumentService excelDocumentService, Stream excelStream, string worksheetName, IEnumerable<T> items, CultureInfo cultureInfo)
        {
            excelDocumentService.Write(excelStream, worksheetName, typeof(T), items, cultureInfo);
        }
    }
}