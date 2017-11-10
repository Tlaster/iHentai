using System.Threading.Tasks;
using iHentai.Services.Core;
using Xunit;
using NApis = iHentai.Services.NHentai.Apis;

namespace iHentai.Test
{
    public class ServiceTest
    {
        [Fact]
        public async Task GalleryTest()
        {
            var apis = ServiceInstances.Instance[ServiceTypes.EHentai];
            var res = await apis.Gallery();
            Assert.NotNull(res.Gallery);
            Assert.NotEmpty(res.Gallery);
        }
    }
}