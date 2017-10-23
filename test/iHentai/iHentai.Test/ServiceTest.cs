using System;
using System.Threading.Tasks;
using iHentai.Services.Core;
using iHentai.Services.EHentai;
using Xunit;
using NApis = iHentai.Services.NHentai.Apis;

namespace iHentai.Test
{
    public class ServiceTest
    {
        [Fact]
        public void TestSearchOption()
        {
            var options = new SearchOption();
            Assert.NotNull(options.ToString());
        }

        [Fact]
        public async Task GalleryTest()
        {
            var apis = ServiceInstances.Instance[ServiceTypes.NHentai];
            var res = await apis.Gallery();
            Assert.NotNull(res);
            Assert.NotEmpty(res);
        }
    }
}