using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OfficeOpenXml;
using Smartiks.Framework.IO.Excel.Abstractions;

namespace Smartiks.Framework.IO.Excel
{
    public class ExcelDocumentService : IExcelDocumentService
    {
        public Task<IList<string>> GetWorksheetNamesAsync(Stream stream)
        {
            using (var package = new ExcelPackage(stream))
            {
                package.Compatibility.IsWorksheets1Based = true;

                return Task.FromResult<IList<string>>(package.Workbook.Worksheets.Select(w => w.Name).ToArray());
            }
        }

        public Task<IList> ReadAsync(Stream stream, string worksheetName, Type type, CultureInfo cultureInfo)
        {
            using (var package = new ExcelPackage(stream))
            {
                package.Compatibility.IsWorksheets1Based = true;

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var propertiesByName =
                    properties
                        .ToDictionary
                        (
                            p => {

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

                    if (cell.Value == null || !(cell.Value is string stringValue) || string.IsNullOrWhiteSpace(stringValue))
                        continue;


                    if (!propertiesByName.TryGetValue(stringValue, out var property))
                        throw new ExcelInvalidHeaderNameException(1, columnNo, cell.Address);

                    columnNoAndPropertyMaps.Add(columnNo, property);
                }

                if (columnNoAndPropertyMaps.Count == 0)
                    throw new ExcelWorksheetEmptyException();

                var items = new ArrayList();

                for (var rowNo = 2; rowNo <= worksheetDimension.Rows; rowNo++)
                {
                    var isOk = false;

                    var item = Activator.CreateInstance(type);

                    foreach (var columnNoAndPropertyMap in columnNoAndPropertyMaps)
                    {
                        var cell = worksheet.Cells[rowNo, columnNoAndPropertyMap.Key];

                        if (cell.Value == null)
                            continue;

                        var propertyType =
                            Nullable.GetUnderlyingType(columnNoAndPropertyMap.Value.PropertyType) ??
                            columnNoAndPropertyMap.Value.PropertyType;

                        object value;

                        try
                        {
                            value = Convert.ChangeType(cell.Value, propertyType, cultureInfo);
                        }
                        catch (InvalidCastException) when (propertyType == typeof(DateTime) && cell.Value is double doubleValue)
                        {
                            value = DateTime.FromOADate(doubleValue);
                        }
                        catch (InvalidCastException ex)
                        {
                            throw new ExcelInvalidCellValueException(rowNo, columnNoAndPropertyMap.Key, cell.Address, columnNoAndPropertyMap.Value.Name, ex);
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

                return Task.FromResult<IList>(items);
            }
        }

        public Task WriteAsync(Stream stream, string worksheetName, IEnumerable items, Type type, CultureInfo cultureInfo)
        {
            using (var package = new ExcelPackage(stream))
            {
                package.Compatibility.IsWorksheets1Based = true;

                var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                var properties =
                    type
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p => {

                            var displayAttribute = p.GetCustomAttribute<DisplayAttribute>();

                            var displayFormatAttribute = p.GetCustomAttribute<DisplayFormatAttribute>();

                            return new {
                                Meta = p,
                                UnderlyingType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType,
                                DisplayName = displayAttribute?.GetName() ?? p.Name,
                                DisplayOrder = displayAttribute?.GetOrder() ?? int.MaxValue,
                                DisplayFormat = displayFormatAttribute?.DataFormatString
                            };
                        })
                        .OrderBy(p => p.DisplayOrder)
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

                        if (!string.IsNullOrWhiteSpace(property.DisplayFormat))
                        {
                            cell.Style.Numberformat.Format = property.DisplayFormat;
                        }

                        columnNo++;
                    }

                    rowNo++;
                }

                package.Save();
            }

            return Task.CompletedTask;
        }
    }
}