using System;
using iHentai.Basic.Helpers;

namespace iHentai.Services
{
    public static class ApiExtensions
    {

        public static T Get<T>(this string value) where T : class
        {
            return Singleton<ApiContainer>.Instance[value] as T;
        }
    }
}