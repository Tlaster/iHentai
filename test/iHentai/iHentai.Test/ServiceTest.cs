using System;
using System.Threading.Tasks;
using iHentai.Services.EHentai;
using Xunit;

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
    }
}