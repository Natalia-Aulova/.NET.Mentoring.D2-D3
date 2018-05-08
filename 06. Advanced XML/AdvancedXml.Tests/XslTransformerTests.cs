using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedXml.Tests
{
    [TestClass]
    public class XslTransformerTests
    {
        [TestMethod]
        public void Test_Transform()
        {
            var transformer = new XslTransformer();
            transformer.Transform("books_valid.xml", "rss_feed.xml");
        }
    }
}
