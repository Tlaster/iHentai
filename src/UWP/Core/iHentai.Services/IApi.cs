using System;
using System.Threading;
using System.Threading.Tasks;

namespace iHentai.Services
{
    public interface IApi
    {
    }

    public interface ILoginData
    {

    }

    public interface ICanLogin
    {
        ILoginData LoginDataGenerator { get; }

        Task<bool> Login(ILoginData data, CancellationToken token = default);
    }

    public interface IInstanceData
    {

    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class StartupAttribute : Attribute
    {

    }
}