using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace MigraineBuddyConvertor.TypeConverters;

public class CustomDateTimeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        // Specify your date format here
        if (DateTime.TryParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
        {
            return dt;
        }
        return base.ConvertFromString(text, row, memberMapData);
    }
}
