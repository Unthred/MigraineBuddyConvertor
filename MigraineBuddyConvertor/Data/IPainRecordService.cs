using Microsoft.AspNetCore.Components.Forms;

namespace MigraineBuddyConvertor.Data;

public interface IPainRecordService
{
    Task<List<PainRecord>?> ImportPainRecords(IBrowserFile? file);
    Task<byte[]> ExportToPdf();
    Task<MemoryStream> ExportToPdfToMemoryStream();
    PainRecord? GetFirstPainRecord();
    PainRecord? GetLastPainRecord();

}