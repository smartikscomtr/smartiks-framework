using System;
using System.Collections.Generic;
using Smartiks.Framework.IO.Abstractions;
using Smartiks.Framework.IO.UnitTests.Models;
using Xunit;
using Xunit.Abstractions;

namespace Smartiks.Framework.IO.UnitTests
{
    public class TypeSerializerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TypeSerializerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> MemberData()
        {
            var assemblyQualifiedNameTypeSerializer = new AssemblyQualifiedNameTypeSerializer();

            var basicTypeSerializer = new BasicTypeSerializer();

            var simpleTypeSerializer = new SimpleTypeSerializer();

            return new List<object[]> {
                new object[] {
                    typeof(Message),
                    assemblyQualifiedNameTypeSerializer
                },
                new object[] {
                    typeof(GenericMessage<Message>),
                    assemblyQualifiedNameTypeSerializer
                },
                new object[] {
                    typeof(Message),
                    basicTypeSerializer
                },
                new object[] {
                    typeof(GenericMessage<Message>),
                    basicTypeSerializer
                },
                new object[] {
                    typeof(Message),
                    simpleTypeSerializer
                },
                new object[] {
                    typeof(GenericMessage<Message>),
                    simpleTypeSerializer
                }
            };
        }

        [Theory]
        [MemberData(nameof(MemberData))]
        public void TypeSerializer(Type type, ITypeSerializer typeSerializer)
        {
            for (var i = 0; i < 100000; i++)
            {
                var serializedType = typeSerializer.Serialize(type);

                _testOutputHelper.WriteLine(serializedType);

                var deserializedType = typeSerializer.Deserialize(serializedType);

                Assert.Equal(type, deserializedType);
            }
        }
    }
}
