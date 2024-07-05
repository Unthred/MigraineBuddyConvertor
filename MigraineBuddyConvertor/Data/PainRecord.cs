using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using MigraineBuddyConvertor.TypeConverters;

namespace MigraineBuddyConvertor.Data;

//public class PainRecord
//{
//    [Name("#")]
//    public int Id { get; set; }

//    [Name("Started")]
//    public string? Started { get; set; }

//    [Name("Lasted")]
//    public string? Lasted { get; set; }

//    [Name("Pain Level")]
//    public string? PainLevel { get; set; }

//    [Name("Affected Activities")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? AffectedActivities { get; set; }

//    [Name("Potential Triggers")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? PotentialTriggers { get; set; }

//    [Name("Symptoms")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? Symptoms { get; set; }

//    [Name("Most Bothersome Symptom")]
//    public string? MostBothersomeSymptom { get; set; }

//    [Name("Premonitory Symptoms")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? PremonitorySymptoms { get; set; }

//    [Name("Pain Positions")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? PainPositions { get; set; } 

//    [Name("Helpful Medication")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? HelpfulMedication { get; set; }

//    [Name("Somewhat Helpful Medication")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? SomewhatHelpfulMedication { get; set; }

//    [Name("Unhelpful Medication")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? UnhelpfulMedication { get; set; }

//    [Name("Unsure Medication")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? UnsureMedication { get; set; }

//    [Name("Helpful Non Drug Relief Methods")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? HelpfulNonDrugReliefMethods { get; set; }

//    [Name("Somewhat Helpful Non Drug Relief Methods")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? SomewhatHelpfulNonDrugReliefMethods { get; set; }

//    [Name("Unhelpful Non Drug Relief Methods")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? UnhelpfulNonDrugReliefMethods { get; set; }

//    [Name("Unsure Non Drug Relief Methods")]
//    [TypeConverter(typeof(CommaSeparatedListConverter))]
//    public List<string>? UnsureNonDrugReliefMethods { get; set; }

//    [Name("Notes")]
//    public string? Notes { get; set; }
//}

public class PainRecord
{
    [Name("#")]
    public int Id { get; set; }

    [Name("Started")]
    [TypeConverter(typeof(DateTimeConverter))]
    public DateTime Started { get; set; }

    [Name("Lasted")]
    [TypeConverter(typeof(CustomTimeSpanConverter))]
    public TimeSpan Lasted { get; set; }

    [Name("Pain Level")]
    public string? PainLevel { get; set; }

    [Name("Affected Activities")]
    public string? AffectedActivities { get; set; }

    [Name("Potential Triggers")]
    public string? PotentialTriggers { get; set; }

    [Name("Symptoms")]
    public string? Symptoms { get; set; }

    [Name("Most Bothersome Symptom")]
    public string? MostBothersomeSymptom { get; set; }

    [Name("Premonitory Symptoms")]
    public string? PremonitorySymptoms { get; set; }

    [Name("Pain Positions")]
    public string? PainPositions { get; set; } 

    [Name("Helpful Medication")]
    public string? HelpfulMedication { get; set; }

    [Name("Somewhat Helpful Medication")]
    public string? SomewhatHelpfulMedication { get; set; }

    [Name("Unhelpful Medication")]
    public string? UnhelpfulMedication { get; set; }

    [Name("Unsure Medication")]
    public string? UnsureMedication { get; set; }

    [Name("Helpful Non Drug Relief Methods")]
    public string? HelpfulNonDrugReliefMethods { get; set; }

    [Name("Somewhat Helpful Non Drug Relief Methods")]
    public string? SomewhatHelpfulNonDrugReliefMethods { get; set; }

    [Name("Unhelpful Non Drug Relief Methods")]
    public string? UnhelpfulNonDrugReliefMethods { get; set; }

    [Name("Unsure Non Drug Relief Methods")]
    public string? UnsureNonDrugReliefMethods { get; set; }

    [Name("Notes")]
    public string? Notes { get; set; }
}
