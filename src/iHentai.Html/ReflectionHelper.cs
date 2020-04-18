using System;
using System.Collections;
using System.Collections.Generic;

namespace iHentai.Html
{
    internal static class ReflectionHelper
    {
        public static Type GetCollectionItemType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (ImplementsGenericDefinition(type, typeof(IEnumerable<>), out var genericListType))
            {
                if (genericListType.IsGenericTypeDefinition)
                {
                    throw new NotSupportedException();
                }

                return genericListType.GetGenericArguments()[0];
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return null;
            }

            throw new NotSupportedException();
        }


        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition,
            out Type implementingType)
        {
            if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentNullException(
                    $"'{genericInterfaceDefinition}' is not a generic interface definition.");
            }

            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    var interfaceDefinition = type.GetGenericTypeDefinition();

                    if (genericInterfaceDefinition == interfaceDefinition)
                    {
                        implementingType = type;
                        return true;
                    }
                }
            }

            foreach (var i in type.GetInterfaces())
            {
                if (!i.IsGenericType)
                {
                    continue;
                }

                var interfaceDefinition = i.GetGenericTypeDefinition();

                if (genericInterfaceDefinition != interfaceDefinition)
                {
                    continue;
                }

                implementingType = i;
                return true;
            }

            implementingType = null;
            return false;
        }
    }
}