using System;
using System.Xml;
using System.Xml.Schema;

namespace AdvancedXml
{
    public class XmlValidator
    {
        private readonly string _targetNamespace = "http://library.by/catalog";
        private readonly string _schema = "booksValidation.xsd";

        public bool Validate(string filePath, Action<int, int, string> errorAction)
        {
            var success = true;
            var settings = new XmlReaderSettings();

            settings.Schemas.Add(_targetNamespace, _schema);

            settings.ValidationEventHandler += 
                (sender, e) =>
                {
                    success = false;
                    errorAction(e.Exception.LineNumber, e.Exception.LinePosition, e.Message);
                };

            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationType = ValidationType.Schema;

            using (var reader = XmlReader.Create(filePath, settings))
            {
                while (reader.Read()) ;
            }

            return success;
        }
    }
}
