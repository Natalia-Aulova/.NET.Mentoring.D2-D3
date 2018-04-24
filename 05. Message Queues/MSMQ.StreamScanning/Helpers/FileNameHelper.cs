using System;
using System.Text.RegularExpressions;
using MSMQ.StreamScanning.Common.Interfaces;
using MSMQ.StreamScanning.Interfaces;

namespace MSMQ.StreamScanning.Helpers
{
    public class FileNameHelper : INameHelper
    {
        private readonly string _digitRegexTemplate = @"(\d{3})";
        private readonly Regex _fileNameRegex;

        public FileNameHelper(ISettingsProvider settingsProvider)
        {
            var supportedExtensions = settingsProvider.GetSupportedExtensions();
            var nameTemplate = settingsProvider.GetNameTemplate();

            var extensionRegexTemplate = $"(?:{string.Join("|", supportedExtensions)})";
            var fileNameRegexTemplate = string.Format(nameTemplate, _digitRegexTemplate, extensionRegexTemplate);

            _fileNameRegex = new Regex(fileNameRegexTemplate, RegexOptions.IgnoreCase);
        }

        public bool IsNameMatch(string fileName)
        {
            return _fileNameRegex.IsMatch(fileName);
        }

        public int GetFileNameNumber(string fileName)
        {
            var match = _fileNameRegex.Match(fileName);

            return int.Parse(match.Groups[1].Value);
        }

        public string GenerateUniqueFileName(string extension)
        {
            return string.Concat(Guid.NewGuid().ToString(), ".", extension);
        }
    }
}
