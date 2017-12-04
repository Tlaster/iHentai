using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHentai.Shared.Helpers
{
    internal static class ReflectionHelper
    {
        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
        {
            if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentNullException($"'{genericInterfaceDefinition}' is not a generic interface definition.");
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
                if (!i.IsGenericType) continue;
                var interfaceDefinition = i.GetGenericTypeDefinition();

                if (genericInterfaceDefinition != interfaceDefinition) continue;
                implementingType = i;
                return true;
            }

            implementingType = null;
            return false;
        }
    }
}
