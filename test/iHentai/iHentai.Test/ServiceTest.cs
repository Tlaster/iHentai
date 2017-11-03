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
        public async Task GalleryTest()
        {
            var apis = ServiceInstances.Instance[ServiceTypes.NHentai];
            var res = await apis.Gallery(searchOption: new Services.NHentai.SearchOption
            {
                Keyword = "taniguchi",
                SearchType = SearchTypes.Keyword
            });
            Assert.NotNull(res.Gallery);
            Assert.NotEmpty(res.Gallery);
        }
    }
}