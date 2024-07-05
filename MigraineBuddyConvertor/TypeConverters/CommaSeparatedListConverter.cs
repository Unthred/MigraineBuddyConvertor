using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace MigraineBuddyConvertor.TypeConverters;

public class CommaSeparatedListConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.Split(',').ToList();
    }
}
