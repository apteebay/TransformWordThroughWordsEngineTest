using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class EType
    {
        public static string GetAssemplyQualifiedName(this Type type) { return type.AssemblyQualifiedName; }
        public static string GetAssemplyQualifiedName(this Type type, bool noVersionInfo)
        {
            if (!noVersionInfo) return type.GetAssemplyQualifiedName();

            if (!type.IsGenericType || type.IsGenericTypeDefinition) return type.GetNamespaceName() + ", " + type.Assembly.GetName().Name;

            var __return = type.GetNamespaceName() + "[";
            var __assemplyQualifiedNames = type.GetGenericArguments().Select(ga => "[" + ga.GetAssemplyQualifiedName(true) + "]").ToArray();
            __return += string.Join(", ", __assemplyQualifiedNames) + "], " + type.Assembly.GetName().Name;
            return __return;
        }

        public static string GetNamespaceName(this Type type)
        {
            if (type.IsNested) return type.DeclaringType.GetNamespaceName() + "+" + type.Name;
            return type.Namespace + "." + type.Name;
        }

    }
}
