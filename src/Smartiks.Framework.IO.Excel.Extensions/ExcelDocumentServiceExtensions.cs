using System;
using System.Collections;
using Smartiks.Framework.IO.Excel.Abstractions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Smartiks.Framework.IO.Excel.Extensions
{
    public static class ExcelDocumentServiceExtensions
    {
        public static async Task<IList<string>> GetWorksheetNamesAsync(this IExcelDocumentService excelDocumentService, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return await excelDocumentService.GetWorksheetNamesAsync(stream);
            }
        }

        public static async Task<IList> ReadAsync(this IExcelDocumentService excelDocumentService, string filePath, string worksheetName, Type type, CultureInfo cultureInfo)
        {
            using (var excelStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return await excelDocumentService.ReadAsync(excelStream, worksheetName, type, cultureInfo);
            }
        }

        public static async Task<IList<T>> ReadAsync<T>(this IExcelDocumentService excelDocumentService, Stream stream, string worksheetName, CultureInfo cultureInfo)
        {
            var result = await excelDocumentService.ReadAsync(stream, worksheetName, typeof(T), cultureInfo);

            return result.Cast<T>().ToArray();
        }

        public static async Task<IList<T>> ReadAsync<T>(this IExcelDocumentService excelDocumentService, string filePath, string worksheetName, CultureInfo cultureInfo)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return await excelDocumentService.ReadAsync<T>(stream, worksheetName, cultureInfo);
            }
        }

        public static async Task WriteAsync(this IExcelDocumentService excelDocumentService, string filePath, string worksheetName, IEnumerable items, Type type, CultureInfo cultureInfo)
        {
            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await excelDocumentService.WriteAsync(stream, worksheetName, items, type, cultureInfo);
            }
        }

        public static Task WriteAsync<T>(this IExcelDocumentService excelDocumentService, Stream stream, string worksheetName, IEnumerable<T> items, CultureInfo cultureInfo)
        {
            return excelDocumentService.WriteAsync(stream, worksheetName, items, typeof(T), cultureInfo);
        }

        public static Task WriteAsync<T>(this IExcelDocumentService excelDocumentService, string filePath, string worksheetName, IEnumerable<T> items, CultureInfo cultureInfo)
        {
            return excelDocumentService.WriteAsync(filePath, worksheetName, items, typeof(T), cultureInfo);
        }
    }
}