using System;
using System.Globalization;

namespace AdvancedXml
{
    public class RssDateTransformer
    {
        public string Transform(string date)
        {
            var dateTime = DateTime.Parse(date);
            return dateTime.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", CultureInfo.GetCultureInfo("en-GB"));
        }
    }
}
