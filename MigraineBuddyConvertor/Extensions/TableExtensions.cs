using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MigraineBuddyConvertor.Extensions;

static class TableExtensions
{
    private static IContainer Cell(this IContainer container, bool dark)
    {
        return container
            .Border(1)
            //.Background(dark ? Colors.Grey.Lighten2 : Colors.White)
            .Background(Colors.White)
            .Padding(2);
    }

    // displays only text label
    public static void LabelCell(this IContainer container, string text, int fontSize = 12) => container.Cell(true).Text(text).Bold().FontSize(fontSize);

    // allows you to inject any type of content, e.g. image
    public static IContainer ValueCell(this IContainer container) => container.Cell(false);
}
