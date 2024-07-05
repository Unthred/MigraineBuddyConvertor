using MigraineBuddyConvertor.Data;

namespace MigraineBuddyConvertor;

internal static class Endpoints
{
    internal static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/downloadPdf", async (HttpContext context, IPainRecordService painRecordService) =>
        {
            var pdfData = await painRecordService.ExportToPdfToMemoryStream();
            return Results.File(pdfData, "application/pdf", "NHS.pdf");
        });

    }
}
