using System;
using System.Reflection;

namespace Smartiks.Framework.IO.Abstractions
{
    public abstract class TypeSerializer : ITypeSerializer
    {
        public abstract string Serialize(Type type);

        public virtual Type Deserialize(string serializedType)
        {
            return Type.GetType(serializedType);

            // return Type.GetType(serializedType, AssemblyResolver, TypeResolver);
        }

        protected virtual Assembly AssemblyResolver(AssemblyName assemblyName)
        {
            // NETFX !NETSTANDARD1_3 return Assembly.LoadWithPartialName(assemblyName);

            return Assembly.Load(assemblyName);
        }

        protected virtual Type TypeResolver(Assembly assembly, string typeName, bool ignoreCase)
        {
            return assembly.GetType(typeName, true, ignoreCase);
        }
    }
}
