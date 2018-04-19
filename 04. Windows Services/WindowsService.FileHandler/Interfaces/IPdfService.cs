using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PdfSharp.Pdf;

namespace WindowsService.FileHandler.Interfaces
{
    public interface IPdfService
    {
        PdfDocument CreateDocument();

        bool AddImage(string imagePath, PdfDocument document);

        void SaveDocument(string destinatonFolderPath, PdfDocument document);

        Task CopyBrokenSequenceToFolder(IList<string> sequence, string pdfFilePath, string destinationFolderPath, CancellationToken token);
    }
}
