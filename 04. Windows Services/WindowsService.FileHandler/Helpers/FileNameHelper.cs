using System;
using System.Text.RegularExpressions;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Helpers
{
    public class FileNameHelper : INameHelper
    {
        private readonly ISettingsProvider _settingsProvider;

        private readonly string _digitRegexTemplate = @"(\d{3})";

        private readonly Regex _fileNameRegex;

        public FileNameHelper(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;

            var supportedExtensions = _settingsProvider
                .GetSetting("SupportedExtensions")
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var nameTemplate = _settingsProvider.GetSetting("NameTemplate");

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
