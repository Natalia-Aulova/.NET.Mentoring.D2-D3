using System.IO;
using System.Xml.Xsl;

namespace AdvancedXml
{
    public class XslTransformer
    {
        private readonly string _stylesheet = "booksToRSS.xslt";
        private readonly string _rssDateTransformer = "rss-date-transformer";

        public void Transform(string inputPath, string outputPath)
        {
            var args = new XsltArgumentList();
            args.AddExtensionObject(_rssDateTransformer, new RssDateTransformer());

            var xsl = new XslCompiledTransform();
            xsl.Load(_stylesheet);
            xsl.Transform(inputPath, args, new FileStream(outputPath, FileMode.Create));
        }
    }
}
