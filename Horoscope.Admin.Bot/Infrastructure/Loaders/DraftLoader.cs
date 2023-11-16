using ClosedXML.Excel;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Models;

namespace Horoscope.Admin.Bot.Infrastructure.Loaders;

public class DraftLoader : IDraftLoader
{
    private const int SkipRowsCount = 1;
    private const int DateCol = 1;
    private const int ZodiacCol = 2;
    private const int ForesightColFrom = 3;
    
    public IEnumerable<Draft> Load(byte[] document)
    {
        using var stream = new MemoryStream(document);
        using var workbook = new XLWorkbook(stream);
        
        var worksheet = workbook.Worksheet(1);
        foreach (var row in worksheet.RowsUsed().Skip(SkipRowsCount))
        {
            var date = row
                .Cell(DateCol)
                .GetDateOrNull();
            var sign = row.Cell(ZodiacCol)
                .GetEnumByDisplayNameOrNull<ZodiacSign>();
            var foresight = LoadForesight(row);
        
            yield return new Draft
            {
                Date = date,
                Sign = sign,
                Foresight = foresight
            };
        }
    }

    private static Foresight LoadForesight(IXLRow row)
    {
        var foresight = new List<string>();
        for (var i = ForesightColFrom; i <= row.LastCellUsed().Address.ColumnNumber; i++)
        {
            if (row.Cell(i).TryGetValue(out string cellValue) && 
                !string.IsNullOrWhiteSpace(cellValue))
            {
                foresight.Add(cellValue);
            }
        }

        return new Foresight(foresight.ToArray());
    }
}