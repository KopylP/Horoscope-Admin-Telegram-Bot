using Horoscope.Admin.Bot.Models;

namespace Horoscope.Admin.Bot.Framework.Contexts;

public sealed class Draft
{
    public Sign Sign { get; set; }
    public DateTime? Date { get; set; }
    public string? Language { get; set; }
    public Foresight? Foresight { get; set; }

    public void Update(Draft secondDraft)
    {
        Sign = secondDraft.Sign;
        Date = secondDraft.Date;
        Language = secondDraft.Language;
        Foresight = secondDraft.Foresight;
    }
}