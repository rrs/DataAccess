using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rrs.Common
{
    public static class TypeExtensions
    {
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            parent = ResolveGenericTypeDefinition(parent);

            var currentChild = child.IsGenericType
                ? child.GetGenericTypeDefinition()
                : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.BaseType != null
                               && currentChild.BaseType.IsGenericType
                    ? currentChild.BaseType.GetGenericTypeDefinition()
                    : currentChild.BaseType;

                if (currentChild == null)
                    return false;
            }
            return false;
        }

        public static bool IsConcreteImplementation(this Type child, Type parent)
        {
            return child.InheritsOrImplements(parent) && !child.IsAbstract;
        }

        public static IEnumerable<Type> ConcreteImplementationsOf(this Type type)
        {
            return Assembly.GetEntryAssembly().GetReferencedAssemblies()
                            .Select(Assembly.Load)
                            .SelectMany(assembly => assembly.GetTypes().Where(t => t.IsConcreteImplementation(type)))
                            .ToList();
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    var currentInterface = childInterface.IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            bool shouldUseGenericType = !(parent.IsGenericType && parent.GetGenericTypeDefinition() != parent);

            if (parent.IsGenericType && shouldUseGenericType) parent = parent.GetGenericTypeDefinition();
            return parent;
        }
    }
}
