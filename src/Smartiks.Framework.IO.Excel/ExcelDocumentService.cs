using OfficeOpenXml;
using Smartiks.Framework.IO.Excel.Abstractions;
using Smartiks.Framework.Text.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Smartiks.Framework.IO.Excel
{
    public class ExcelDocumentService : IExcelDocumentService
    {
        private readonly IStringConverterService _stringConverterService;

        public ExcelDocumentService(IStringConverterService stringConverterService)
        {
            _stringConverterService = stringConverterService;
        }

        public IReadOnlyCollection<string> GetWorksheetNames(Stream excelStream)
        {
            using (var package = new ExcelPackage(excelStream))
            {
                package.Compatibility.IsWorksheets1Based = true;

                return package.Workbook.Worksheets.Select(w => w.Name).ToList().AsReadOnly();
            }
        }

        public IReadOnlyCollection<object> Read(Stream excelStream, string worksheetName, Type type, CultureInfo cultureInfo)
        {
            using (var package = new ExcelPackage(excelStream))
            {
                package.Compatibility.IsWorksheets1Based = true;

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var propertiesByName =
                    properties
                        .ToDictionary
                        (
                            p =>
                            {
                                var displayAttribute = p.GetCustomAttribute<DisplayAttribute>();

                                return displayAttribute?.GetName() ?? p.Name;
                            },
                            StringComparer.Create(cultureInfo, true)
                        );

                var worksheet = package.Workbook.Worksheets[worksheetName];

                var worksheetDimension = worksheet.Dimension;

                if (worksheetDimension == null)
                    throw new ExcelWorksheetEmptyException();

                var columnNoAndPropertyMaps = new Dictionary<int, PropertyInfo>();

                for (var columnNo = 1; columnNo <= worksheetDimension.Columns; columnNo++)
                {
                    var cell = worksheet.Cells[1, columnNo];

                    if (string.IsNullOrWhiteSpace(cell.Text))
                        continue;

                    if (!propertiesByName.TryGetValue(cell.Text, out var property))
                        throw new ExcelInvalidHeaderNameException(1, columnNo, cell.Address);

                    columnNoAndPropertyMaps.Add(columnNo, property);
                }

                if (columnNoAndPropertyMaps.Count == 0)
                    throw new ExcelWorksheetEmptyException();

                var items = new List<object>();

                for (var rowNo = 2; rowNo <= worksheetDimension.Rows; rowNo++)
                {
                    var isOk = false;

                    var item = Activator.CreateInstance(type);

                    foreach (var columnNoAndPropertyMap in columnNoAndPropertyMaps)
                    {
                        var cell = worksheet.Cells[rowNo, columnNoAndPropertyMap.Key];

                        if (string.IsNullOrWhiteSpace(cell.Text))
                            continue;

                        var propertyType =
                            Nullable.GetUnderlyingType(columnNoAndPropertyMap.Value.PropertyType) ??
                            columnNoAndPropertyMap.Value.PropertyType;

                        object value;

                        try
                        {
                            value = ChangeType(cell.Text, propertyType, cultureInfo);
                        }
                        catch (FormatException ex)
                        {
                            throw new ExcelInvalidCellValueException(rowNo, columnNoAndPropertyMap.Key, cell.Address, columnNoAndPropertyMap.Value.Name, ex);
                        }

                        if (value != null)
                            columnNoAndPropertyMap.Value.SetValue(item, value);

                        isOk = true;
                    }

                    if (isOk)
                    {
                        items.Add(item);
                    }
                }

                return items.AsReadOnly();
            }
        }

        public void Write(Stream excelStream, string worksheetName, Type type, IEnumerable items, CultureInfo cultureInfo)
        {
            using (var package = new ExcelPackage(excelStream))
            {
                package.Compatibility.IsWorksheets1Based = true;

                var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                var properties =
                    type
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p =>
                        {
                            var displayAttribute = p.GetCustomAttribute<DisplayAttribute>();

                            return new
                            {
                                Meta = p,
                                DisplayName = displayAttribute?.GetName() ?? p.Name,
                                Order = displayAttribute?.GetOrder() ?? int.MaxValue,
                                UnderlyingType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType
                            };
                        })
                        .OrderBy(p => p.Order)
                        .ToArray();

                var columnNo = 1;

                foreach (var property in properties)
                {
                    var cell = worksheet.Cells[1, columnNo];

                    cell.Value = property.DisplayName;

                    columnNo++;
                }

                var rowNo = 2;

                foreach (var item in items)
                {
                    columnNo = 1;

                    foreach (var property in properties)
                    {
                        var cell = worksheet.Cells[rowNo, columnNo];

                        cell.Value = property.Meta.GetValue(item);

                        if (property.UnderlyingType == typeof(DateTime) || property.UnderlyingType == typeof(DateTimeOffset))
                        {
                            cell.Style.Numberformat.Format = $"{cultureInfo.DateTimeFormat.ShortDatePattern} {cultureInfo.DateTimeFormat.ShortTimePattern}";
                        }

                        columnNo++;
                    }

                    rowNo++;
                }

                package.Save();
            }
        }

        private object ChangeType(string value, Type targetType, IFormatProvider formatProvider)
        {
            return
                _stringConverterService != null
                    ? _stringConverterService.FromString(value, targetType, formatProvider)
                    : Convert.ChangeType(value, targetType, formatProvider);
        }
    }
}