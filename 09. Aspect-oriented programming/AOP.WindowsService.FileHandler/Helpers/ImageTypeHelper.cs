using System.Linq;

namespace AOP.WindowsService.FileHandler.Helpers
{
    public static class ImageTypeHelper
    {
        private static readonly byte[] _png = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] _jpg = new byte[] { 0xFF, 0xD8, 0xFF };
        private static readonly byte[] _bmp = new byte[] { 0x42, 0x4D };

        public static bool IsImage(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return false;
            }

            if (_png.SequenceEqual(bytes.Take(_png.Length)) ||
                _jpg.SequenceEqual(bytes.Take(_jpg.Length)) ||
                _bmp.SequenceEqual(bytes.Take(_bmp.Length)))
            {
                return true;
            }

            return false;
        }
    }
}
