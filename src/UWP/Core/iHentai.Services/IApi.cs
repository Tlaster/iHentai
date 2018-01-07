using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Basic.Helpers;

namespace iHentai.Services
{
    public interface IApi
    {
    }

    public interface IHttpHandler
    {
        bool Handle(ref HttpRequestMessage message);
    }
    
    public interface ILoginData
    {
    }

    public interface ICanLogin
    {
        ILoginData LoginDataGenerator { get; }

        Task<IInstanceData> Login(ILoginData data, CancellationToken token = default);

        Type InstanceDataType { get; }
    }

    public interface ISingletonLogin : ICanLogin
    {

    }

    public interface IInstanceData
    {
    }

    public static class InstanceDataExtensions
    {
        public static T Get<T>(this Guid data) where T : class, IInstanceData
        {
            return Singleton<ApiContainer>.Instance[data] as T;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class StartupAttribute : Attribute
    {
    }
}