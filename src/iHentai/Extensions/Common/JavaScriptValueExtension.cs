using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Extensions.Hosting;
using Microsoft.VisualBasic.CompilerServices;

namespace iHentai.Extensions.Common
{
  
    internal static class JavaScriptValueExtension
    {
        public static JavaScriptPropertyId ToJavaScriptPropertyId(this string id)
        {
            return JavaScriptPropertyId.FromString(id);
        }

        public static JavaScriptValue ToJavaScriptValue(this object it)
        {
            return it switch
            {
                JavaScriptValue javaScriptValue => javaScriptValue,
                bool value => JavaScriptValue.FromBoolean(value),
                string value => JavaScriptValue.FromString(value),
                int value => JavaScriptValue.FromInt32(value),
                double value => JavaScriptValue.FromDouble(value),
                float value => JavaScriptValue.FromDouble(value),
                decimal value => JavaScriptValue.FromDouble(Convert.ToDouble(value)),
                null => JavaScriptValue.Null,
                _ => JavaScriptValue.Invalid
            };
        }

        public static T ToNative<T>(this JavaScriptValue value)
        {
            return value.ToNative() is T ? (T) value.ToNative() : default;
        }

        public static object? ToNative(this JavaScriptValue value)
        {
            return value.ValueType switch
            {
                JavaScriptValueType.Number => value.ToDouble()
                    .Let(it => Math.Abs(it % 1) <= double.Epsilon * 100 ? Convert.ToInt32(it) : it),
                JavaScriptValueType.String => value.ToString(),
                JavaScriptValueType.Boolean => value.ToBoolean(),
                _ => null
            };
        }
    }
}
