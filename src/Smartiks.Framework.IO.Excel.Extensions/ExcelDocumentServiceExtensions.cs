using System;
using System.Collections;
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

        public static IReadOnlyCollection<object> Read(this IExcelDocumentService excelDocumentService, string excelFilePath, string worksheetName, Type type, CultureInfo cultureInfo)
        {
            using (var excelFileStream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return excelDocumentService.Read(excelFileStream, worksheetName, type, cultureInfo);
            }
        }

        public static IReadOnlyCollection<T> Read<T>(this IExcelDocumentService excelDocumentService, string excelFilePath, string worksheetName, CultureInfo cultureInfo)
        {
            return
                excelDocumentService
                    .Read(excelFilePath, worksheetName, typeof(T), cultureInfo)
                    .Cast<T>()
                    .ToList()
                    .AsReadOnly();
        }

        public static void Write<T>(this IExcelDocumentService excelDocumentService, Stream excelStream, string worksheetName, IEnumerable<T> items, CultureInfo cultureInfo)
        {
            excelDocumentService.Write(excelStream, worksheetName, typeof(T), items, cultureInfo);
        }

        public static void Write(this IExcelDocumentService excelDocumentService, string excelFilePath, string worksheetName, Type type, IEnumerable items, CultureInfo cultureInfo)
        {
            using (var excelFileStream = File.Open(excelFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                excelDocumentService.Write(excelFileStream, worksheetName, type, items, cultureInfo);
            }
        }

        public static void Write<T>(this IExcelDocumentService excelDocumentService, string excelFilePath, string worksheetName, IEnumerable<T> items, CultureInfo cultureInfo)
        {
            excelDocumentService.Write(excelFilePath, worksheetName, typeof(T), items, cultureInfo);
        }
    }
}