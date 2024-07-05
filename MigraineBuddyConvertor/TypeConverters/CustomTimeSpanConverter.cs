using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using QuestPDF.Infrastructure;

namespace MigraineBuddyConvertor.TypeConverters;

public class CustomTimeSpanConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == "Ongoing")
        {
            return TimeSpan.Zero;
        }
        if (text.Contains("\n"))
        {
            text = text[..text.IndexOf('\n')];
        }
        
        // Remove the 'h' and 'm' characters and split the string into hours and minutes
        var parts = text.Replace("h", "").Replace("m", "").Split(' ');

        // Parse the hours and minutes into a TimeSpan
        if (parts.Length == 2
            && int.TryParse(parts[0], out var hours)
            && int.TryParse(parts[1], out var minutes))
        {
            return new TimeSpan(hours, minutes, 0);
        }

        return base.ConvertFromString(text, row, memberMapData);
    }
}
