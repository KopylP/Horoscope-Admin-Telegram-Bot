namespace Horoscope.Admin.Bot.Models;

public sealed class Draft
{
    public ZodiacSign Sign { get; set; }
    public DateTime? Date { get; set; }
    // private string? Language { get; set; }
    public Foresight? Foresight { get; set; }
    public bool IsReadyForPublish => IsReadyForPublishDraft();

    public void Update(Draft secondDraft)
    {
        Sign = secondDraft.Sign;
        Date = secondDraft.Date;
        // Language = secondDraft.Language;
        Foresight = secondDraft.Foresight;
    }

    private bool IsReadyForPublishDraft() =>
        Sign != ZodiacSign.None &&
        Date.HasValue &&
        Foresight is not null && !Foresight.IsEmpty;
}