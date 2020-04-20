using System;
using System.Text;
using Smartiks.Framework.IO.Abstractions;

namespace Smartiks.Framework.IO
{
    public class BasicTypeSerializer : TypeSerializer
    {
        public override string Serialize(Type type)
        {
            var builder = new StringBuilder(128);

            var writingAssemblyName = false;

            var skippingAssemblyDetails = false;

            foreach (var character in type.AssemblyQualifiedName)
            {
                switch (character)
                {
                    case '[':
                        writingAssemblyName = false;

                        skippingAssemblyDetails = false;

                        builder.Append(character);

                        break;

                    case ']':
                        writingAssemblyName = false;

                        skippingAssemblyDetails = false;

                        builder.Append(character);

                        break;

                    case ',':
                        if (!writingAssemblyName)
                        {
                            writingAssemblyName = true;

                            builder.Append(character);
                        }
                        else
                            skippingAssemblyDetails = true;

                        break;

                    default:
                        if (!skippingAssemblyDetails)
                            builder.Append(character);

                        break;
                }
            }

            return builder.ToString();
        }
    }
}