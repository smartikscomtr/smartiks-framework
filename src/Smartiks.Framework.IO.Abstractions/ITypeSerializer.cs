using System;

namespace Smartiks.Framework.IO.Abstractions
{
    public interface ITypeSerializer
    {
        string Serialize(Type type);

        Type Deserialize(string serializedType);
    }
}
