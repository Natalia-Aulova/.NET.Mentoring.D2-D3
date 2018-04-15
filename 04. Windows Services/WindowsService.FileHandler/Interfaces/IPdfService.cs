using PdfSharp.Pdf;

namespace WindowsService.FileHandler.Interfaces
{
    public interface IPdfService
    {
        PdfDocument CreateDocument();

        void AddImage(string imagePath, PdfDocument document);

        void SaveDocument(string destinatonFolderPath, PdfDocument document);
    }
}
