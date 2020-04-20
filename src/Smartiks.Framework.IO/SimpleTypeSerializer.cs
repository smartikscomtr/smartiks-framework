using System;
using System.Text.RegularExpressions;
using Smartiks.Framework.IO.Abstractions;

namespace Smartiks.Framework.IO
{
    public class SimpleTypeSerializer : TypeSerializer
    {
        public override string Serialize(Type type)
        {
            return Regex.Replace(type.AssemblyQualifiedName, @", Version=\d+.\d+.\d+.\d+, Culture=[\w-]+, PublicKeyToken=(?:null|[a-f0-9]{16})", string.Empty);
        }
    }
}
