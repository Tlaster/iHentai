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

            return value.ToString();
        }
    }
}