using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Smartiks.Framework.IO.Excel.UnitTests.Models;
using Smartiks.Framework.IO.Excel.Extensions;
using Xunit;

namespace Smartiks.Framework.IO.Excel.UnitTests
{
    public class ExcelDocumentServiceTest
    {
        private readonly ExcelDocumentService excelDocumentService;

        public ExcelDocumentServiceTest()
        {
            excelDocumentService = new ExcelDocumentService();
        }
        

        public static IEnumerable<object[]> ReadFromFileData()
        {
            return new List<object[]> {
                new object[] {
                    "Data/excel-model.xlsx",
                    "Worksheet",
                    CultureInfo.CurrentCulture,
                    new [] {
                        new ExcelModel {
                            IntValue = 1,
                            NullableIntValue = 1,
                            DecimalValue = 1.5M,
                            NullableDecimalValue = 1.5M,
                            DateTimeValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            NullableDateTimeValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            DateValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            NullableDateValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            StringValue = "Text"
                        }
                    }
                }
            };
        }

        public static IEnumerable<object[]> WriteToFileData()
        {
            return new List<object[]> {
                new object[] {
                    "Data/excel-model-test.xlsx",
                    "Worksheet",
                    new [] {
                        new ExcelModel {
                            IntValue = 1,
                            NullableIntValue = 1,
                            DecimalValue = 1.5M,
                            NullableDecimalValue = 1.5M,
                            DateTimeValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            NullableDateTimeValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            DateValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            NullableDateValue = new DateTime(2018, 2, 1, 23, 10, 10),
                            StringValue = "Text"
                        }
                    },
                    CultureInfo.InvariantCulture
                }
            };
        }


        [Theory]
        [MemberData(nameof(ReadFromFileData))]
        public async Task ReadFromFile(string filePath, string worksheetName, CultureInfo cultureInfo, ExcelModel[] expectedExcelModels)
        {
            var excelModels = await excelDocumentService.ReadAsync<ExcelModel>(filePath, worksheetName, cultureInfo);

            Assert.Equal(expectedExcelModels, excelModels);
        }

        [Theory]
        [MemberData(nameof(WriteToFileData))]
        public async Task WriteToFile(string filePath, string worksheetName, ExcelModel[] excelModels, CultureInfo cultureInfo)
        {
            await excelDocumentService.WriteAsync(filePath, worksheetName, excelModels, cultureInfo);
        }
    }
}
