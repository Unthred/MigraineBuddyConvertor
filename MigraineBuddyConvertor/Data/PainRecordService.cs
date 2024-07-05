using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using MigraineBuddyConvertor.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace MigraineBuddyConvertor.Data;

public class PainRecordService : IPainRecordService
{
    private const int TableHeaderFontSize = 10;
    private const int TableRowFontSize = 10;
    private const int HeaderFontSize = 18;
    private const int InstructionsFontSize = 12;
    private const int ReminderFontSize = 12;
    private const int RightTextPadding = 35;
    private const int LeftTextPadding = 35;
    private const int DateWidth = 70;
    private const int WeekBeginningWidth = 70;
    private const int LastedWidth = 100;
    private const int MedicationWidth = 300;
    private const int PainScoreWidth = 100;
    private const float LineHeight = 0.8f;

    private List<PainRecord>? _painRecords;

    public PainRecord? GetFirstPainRecord()
    {
        return _painRecords?.First();
    }

    public PainRecord? GetLastPainRecord()
    {
        return _painRecords?.Last();
    }

    public async Task<List<PainRecord>?> ImportPainRecords(IBrowserFile? file)
    {
        return await LoadPainRecords(file);
    }

    private async Task<List<PainRecord>?> LoadPainRecords(IBrowserFile? file)
    {
        if (file == null) 
            return null;

        var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            
            HasHeaderRecord = false,
            IgnoreBlankLines = true,
            AllowComments = true,
            Delimiter = ",",
            TrimOptions = TrimOptions.Trim,
            BadDataFound = context =>
            {
                Console.WriteLine($"Bad data found on {context.RawRecord}");
            },

        };


        using var csv = new CsvReader(reader, config);

        // Skip the first 4 lines
        for (int i = 0; i < 3; i++)
        {
            await csv.ReadAsync();
        }

        var rawPainRecords = new List<PainRecord>();
        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<PainRecord>();
            rawPainRecords.Add(record);
        }

        //TODO why does this not work?
        // var records = csv.GetRecordsAsync<PainRecord>();
        // await foreach (var record in records)
        // {
        //     painRecords.Add(record);
        // }
        //_firstPainRecord = _painRecords.Select()

        var orderedPainRecords = rawPainRecords.OrderBy(x => x.Started).ToList();
        return ProcessPainRecords(orderedPainRecords);

    }

    private List<PainRecord>? ProcessPainRecords(List<PainRecord> rawPainRecords)
    {
        // Group the records by date
        var groupedRecords = rawPainRecords.GroupBy(r => r.Started.Date);

        // Create a new list to hold the combined records
        var combinedRecords = new List<PainRecord>();

        foreach (var group in groupedRecords)
        {
            // Create a new record with the summed time span and the highest pain level
            var combinedRecord = new PainRecord
            {
                Started = group.Key,
                Lasted = new TimeSpan(group.Sum(r => r.Lasted.Ticks)),
                PainLevel = group.Max(r => r.PainLevel),
                // Copy other fields from one of the records, or compute them as needed
                //HelpfulMedication = string.Join(", ", group.Select(r => r.HelpfulMedication)),
                HelpfulMedication = string.Join(", ", group.Select(r => 
                    string.Join(", ", new[] { r.HelpfulMedication, r.SomewhatHelpfulMedication, r.UnhelpfulMedication, r.UnsureMedication }
                        .Where(s => !string.IsNullOrEmpty(s))))),
            };

            // Add the combined record to the list
            combinedRecords.Add(combinedRecord);

            // Check if the combined record lasted more than 24 hours
            var nextDay = combinedRecord;
            do 
            {
                // If the combined record lasted more than 24 hours, add a new record for the next day
                nextDay = AddAdditionalDay(nextDay);
                if (nextDay is not null)
                {
                    combinedRecords.Add(nextDay);
                }

            } while (nextDay is not null);


        }

        // Replace the old list with the new one
        _painRecords = combinedRecords;
        return _painRecords;
    }

    private PainRecord? AddAdditionalDay(PainRecord combinedRecord)
    {
        var twentyFourHours = TimeSpan.FromHours(24);
        if (combinedRecord.Lasted <= twentyFourHours) return null;
        // Calculate the remainder
        var remainder = combinedRecord.Lasted - twentyFourHours;

        // Adjust the current record
        combinedRecord.Lasted = twentyFourHours - TimeSpan.FromHours(combinedRecord.Started.Hour + combinedRecord.Started.Minute / 60.0);

        // Create a new record for the next day with the remainder
        var nextDayRecord = new PainRecord
        {
            Started = combinedRecord.Started.AddDays(1),
            Lasted = remainder,
            PainLevel = combinedRecord.PainLevel
        };
        return nextDayRecord;
    }


    public async Task<byte[]> ExportToPdf()
    {
        const string reminder = "Please remember to bring your diaries with you to your appointment";
        const string reminderWithAddon = $"{reminder}, without this information treatment may be delayed";
        const string instructions = "Is is necessary for you to complete this headache diary for three to four months in order we may evaluate your headache treatment";

        var groupedRecords = _painRecords.GroupBy(r => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(r.Started, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToList();

        int tableCount = 0;

        var document = Document.Create(document =>
        {
            for (int i = 0; i < groupedRecords.Count(); i+=4)
            {
                if (tableCount % 4 == 0)
                {
                    var i1 = i;
                    document.Page(page =>
                    {
                        // page content

                        page.Margin(1, Unit.Centimetre);
                        page.Header().AlignRight().Row(row =>
                        {
                            row.ConstantItem(100).Element(logo =>
                            {
                                logo.Image("NHSlogo.png");
                            });
                        });

                        page.Content()
                            .Column(column =>
                            {
                                column.Item()
                                    .Text("Headache Diary")
                                    .AlignCenter()
                                    .FontFamily("Times New Roman")
                                    .Bold()
                                    .Underline()
                                    .LineHeight(LineHeight)
                                    .FontSize(HeaderFontSize);
                                column.Item()
                                    .PaddingLeft(LeftTextPadding)
                                    .PaddingRight(RightTextPadding)
                                    .Text(instructions)
                                    .FontFamily("Times New Roman")
                                    .LineHeight(LineHeight)
                                    .FontSize(InstructionsFontSize);
                                column.Item()
                                    .PaddingLeft(LeftTextPadding)
                                    .PaddingRight(RightTextPadding)
                                    .Text(reminderWithAddon)
                                    .FontFamily("Times New Roman")
                                    .Bold()
                                    .Underline()
                                    .LineHeight(LineHeight)
                                    .FontSize(ReminderFontSize);
                                column.Item()
                                    .PaddingLeft(LeftTextPadding)
                                    .PaddingRight(RightTextPadding)
                                    .Text(reminder)
                                    .FontFamily("Times New Roman")
                                    .Bold()
                                    .Underline()
                                    .FontSize(ReminderFontSize)
                                    .AlignCenter();

                                // Add the table for the week here
                                var week1 = (i1 < groupedRecords.Count) ? groupedRecords[i1] : null;
                                var week2 = (i1 + 1 < groupedRecords.Count) ? groupedRecords[i1 + 1] : null;
                                var week3 = (i1 + 2 < groupedRecords.Count) ? groupedRecords[i1 + 2] : null;
                                var week4 = (i1 + 3 < groupedRecords.Count) ? groupedRecords[i1 + 3] : null;

                                if (week1 != null) AddTableForWeek(column, week1);
                                if (week2 != null) AddTableForWeek(column, week2);
                                if (week3 != null) AddTableForWeek(column, week3);
                                if (week4 != null) AddTableForWeek(column, week4);
                            });
                        page.Footer().Element(footer =>
                        {
                            footer.Text("Pain score 0=none     _    1-3= mild          4-6 = moderate          7-10 = severe")  
                                .FontSize(10)
                                .Bold()
                                .AlignCenter();
                        });
                    });
                }

                tableCount++;
            }
        });

        return document.GeneratePdf();

    }

    public async Task<MemoryStream> ExportToPdfToMemoryStream()
    {
        var pdf = await ExportToPdf();

        MemoryStream stream = new MemoryStream(pdf);

        return stream;
    }

    //private string GetContentType(string path)
    //{
    //    var types = GetMimeTypes();
    //    var ext = Path.GetExtension(path).ToLowerInvariant();
    //    return types[ext];
    //}

    //private Dictionary<string, string> GetMimeTypes()
    //{
    //    return new Dictionary<string, string>
    //    {
    //        {".txt", "text/plain"},
    //        {".pdf", "application/pdf"},
    //        // other document formats
    //    };
    //}


    private static void AddTableForWeek(ColumnDescriptor column, IGrouping<int, PainRecord> group)
    {
        column.Spacing(10); // Adjust the number to add more or less space
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                //columns.ConstantColumn(DateWidth);
                columns.ConstantColumn(WeekBeginningWidth);
                columns.ConstantColumn(LastedWidth);
                columns.RelativeColumn(MedicationWidth);
                columns.RelativeColumn(PainScoreWidth);
            });

            //table.Cell().LabelCell("Date", TableHeaderFontSize);
            table.Cell().LabelCell("Week Beginning", TableHeaderFontSize);
            table.Cell().LabelCell("How long did your headache last?", TableHeaderFontSize);
            table.Cell().LabelCell("Please List medication taken", TableHeaderFontSize);
            table.Cell().LabelCell("Average Pain Score", TableHeaderFontSize);

            // Create a list of all days in the week
            var daysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            foreach (var day in daysOfWeek)
            {
                // Find the record for the current day
                var painRecord = group.FirstOrDefault(record => record.Started.DayOfWeek == day);

                if (painRecord != null)
                {
                    // If there is a record for the current day, output the record
                    //table.Cell().ValueCell().AlignLeft().Text(painRecord.Started.ToString()).FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignLeft().Text(painRecord.Started.DayOfWeek.ToString()).FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignLeft().Text($"{(int)painRecord.Lasted.TotalHours}h {painRecord.Lasted.Minutes}m").FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignLeft().Text(painRecord.HelpfulMedication).FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignCenter().Text(painRecord.PainLevel).FontSize(TableRowFontSize);
                }
                else
                {
                    // If there is no record for the current day, output an empty row
                    //table.Cell().ValueCell().AlignLeft().Text(day.ToString()).FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignLeft().Text(day.ToString()).FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignLeft().Text("").FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignLeft().Text("").FontSize(TableRowFontSize);
                    table.Cell().ValueCell().AlignCenter().Text("").FontSize(TableRowFontSize);
                }
            }

        });
    }
}
