using System.Collections.Generic;
using System.Reflection;

namespace iHentai.Services
{
    public interface IApiApplication
    {
        IEnumerable<Assembly> GetApiAssemblies();

        IEnumerable<(Assembly ApiAssembly, Assembly ViewAssembly)> GetEntries();
    }
}