using System;
using iHentai.Apis.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iHentai.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {
            var res = await ServiceInstances.Instance[ServiceTypes.NHentai].Gallery();
            Assert.IsNotNull(res.Gallery);
        }
    }
}
