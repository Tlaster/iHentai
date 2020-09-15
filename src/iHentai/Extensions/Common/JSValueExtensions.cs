//using NiL.JS.Core;
//using NiL.JS.Extensions;

//namespace iHentai.Extensions.Common
//{
//    internal static class JSValueExtensions
//    {
//        public static bool TryGet<T>(this JSValue value, string name, out T result)
//        {
//            var property = value.GetProperty(name);
//            if (property.Exists)
//            {
//                result = property.As<T>();
//                return true;
//            }

//            result = default;
//            return false;
//        }
//    }
//}