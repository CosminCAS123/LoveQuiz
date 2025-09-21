using LoveQuiz.Server.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure; // core types like Document, IDocument, IContainer
using QuestPDF.Helpers;

namespace LoveQuiz.Server.Services
{
    public class PdfGenerationService
    {
        private static readonly SemaphoreSlim _gate = new(2, 2);

        public async Task<byte[]> GenerateAsync(FinalReport dto, CancellationToken ct = default)
        {
            await _gate.WaitAsync(ct);
            try
            {
                using var ms = new MemoryStream();
                // Build a fresh Document per call; do NOT store it on fields.
                Document.Create(container =>
                {
                    // ... compose using dto
                })
                .GeneratePdf(ms); // CPU-bound, synchronous

                return ms.ToArray();
            }
            finally
            {
                _gate.Release();
            }
        }
    }
}
