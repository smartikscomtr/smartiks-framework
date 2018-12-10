using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Smartiks.Framework.IO.Xml.Extensions;
using Smartiks.Framework.IO.Xml.UnitTests.Models;

namespace Smartiks.Framework.IO.Xml.UnitTests
{
    public class XmlDocumentServiceTest
    {
        private static readonly Encoding UTF8EncodingWithoutBom = new UTF8Encoding(false, true);


        private readonly XmlDocumentService xmlDocumentService;

        public XmlDocumentServiceTest()
        {
            xmlDocumentService = new XmlDocumentService();
        }
        

        public static IEnumerable<object[]> ReadFromFileData()
        {
            return new List<object[]> {
                new object[] {
                    "Data/xml-model.xml",
                    UTF8EncodingWithoutBom, 
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    "Data/xml-model-utf-8.xml",
                    UTF8EncodingWithoutBom,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    "Data/xml-model-utf-8-bom.xml",
                    Encoding.UTF8,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    "Data/xml-model-utf-16.xml",
                    Encoding.Unicode,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    "Data/xml-model-utf-16BE.xml",
                    Encoding.BigEndianUnicode,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                }
            };
        }

        public static IEnumerable<object[]> ReadFromStringData()
        {
            return new List<object[]> {
                new object[] {
                    File.ReadAllText("Data/xml-model.xml"),
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    File.ReadAllText("Data/xml-model-utf-8.xml"),
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    File.ReadAllText("Data/xml-model-utf-8-bom.xml"),
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    File.ReadAllText("Data/xml-model-utf-16.xml"),
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                },
                new object[] {
                    File.ReadAllText("Data/xml-model-utf-16BE.xml"),
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    }
                }
            };
        }

        public static IEnumerable<object[]> WriteToFileData()
        {
            return new List<object[]> {
                new object[] {
                    "Data/xml-model-utf-8-test.xml",
                    UTF8EncodingWithoutBom,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    "Data/xml-model-utf-8.xml"
                },
                new object[] {
                    "Data/xml-model-utf-8-test.xml",
                    Encoding.UTF8,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    "Data/xml-model-utf-8-bom.xml"
                },
                new object[] {
                    "Data/xml-model-utf-16-test.xml",
                    Encoding.Unicode,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    "Data/xml-model-utf-16.xml"
                },
                new object[] {
                    "Data/xml-model-utf-16BE-test.xml",
                    Encoding.BigEndianUnicode,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    "Data/xml-model-utf-16BE.xml"
                }
            };
        }

        public static IEnumerable<object[]> WriteToStringData()
        {
            return new List<object[]> {
                new object[] {
                    UTF8EncodingWithoutBom, 
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    File.ReadAllText("Data/xml-model-utf-8.xml")
                },
                new object[] {
                    Encoding.UTF8,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    File.ReadAllText("Data/xml-model-utf-8-bom.xml")
                },
                new object[] {
                    Encoding.Unicode,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    File.ReadAllText("Data/xml-model-utf-16.xml")
                },
                new object[] {
                    Encoding.BigEndianUnicode,
                    new XmlModel {
                        IntValue = 1,
                        NullableIntValue = 1,
                        DecimalValue = 1.5M,
                        NullableDecimalValue = 1.5M,
                        StringValue = "Text"
                    },
                    File.ReadAllText("Data/xml-model-utf-16BE.xml")
                }
            };
        }


        [Theory]
        [MemberData(nameof(ReadFromFileData))]
        public async Task ReadFromFile(string filePath, Encoding encoding, XmlModel expectedXmlModel)
        {
            var xmlModel = await xmlDocumentService.ReadAsync(filePath, encoding, typeof(XmlModel)) as XmlModel;

            Assert.Equal(expectedXmlModel, xmlModel);
        }

        [Theory]
        [MemberData(nameof(ReadFromStringData))]
        public async Task ReadFromString(string xml, XmlModel expectedXmlModel)
        {
            var xmlModel = await xmlDocumentService.ReadAsync(xml, typeof(XmlModel)) as XmlModel;

            Assert.Equal(expectedXmlModel, xmlModel);
        }

        [Theory]
        [MemberData(nameof(WriteToFileData))]
        public async Task WriteToFile(string filePath, Encoding encoding, XmlModel xmlModel, string expectedFilePath)
        {
            await xmlDocumentService.WriteAsync(filePath, encoding, xmlModel, typeof(XmlModel));

            Assert.Equal(await File.ReadAllBytesAsync(filePath), await File.ReadAllBytesAsync(expectedFilePath));
        }

        [Theory]
        [MemberData(nameof(WriteToStringData))]
        public async Task WriteToString(Encoding encoding, XmlModel xmlModel, string expectedXml)
        {
            var xml = await xmlDocumentService.WriteAsync(encoding, xmlModel, typeof(XmlModel));

            Assert.Equal(expectedXml, xml);
        }
    }
}
