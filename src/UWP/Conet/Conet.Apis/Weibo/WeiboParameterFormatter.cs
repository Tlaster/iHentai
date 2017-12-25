using System;
using System.Reflection;
using Refit;

namespace Conet.Apis.Weibo
{
    internal class WeiboParameterFormatter : IUrlParameterFormatter
    {
        public string Format(object value, ParameterInfo parameterInfo)
        {
            if (value == null)
            {
                return null;
            }

            if (parameterInfo.ParameterType.IsEnum)
            {
                return ((Enum)value).ToString("D");
            }

            if (parameterInfo.ParameterType == typeof(bool))
            {
                return Convert.ToInt32(value).ToString();
            }
            return value.ToString();
        }
    }
}