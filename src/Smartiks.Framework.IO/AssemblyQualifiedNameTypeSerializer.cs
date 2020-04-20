using System;
using Smartiks.Framework.IO.Abstractions;

namespace Smartiks.Framework.IO
{
    public class AssemblyQualifiedNameTypeSerializer : TypeSerializer
    {
        public override string Serialize(Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}