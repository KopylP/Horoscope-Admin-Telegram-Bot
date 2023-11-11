using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Framework.Keyword;

public class ReplyKeyboardMarkupBuilder
{
    private readonly List<List<KeyboardButton>> _keyboard = new();
    
    private bool _resizeKeyboard = true;
    private bool _oneTimeKeyboard = true;
    private bool _selective = true;

    private ReplyKeyboardMarkupBuilder()
    {
    }

    public static ReplyKeyboardMarkupBuilder Create()
        => new ReplyKeyboardMarkupBuilder();

    public ReplyKeyboardMarkupBuilder AddRow(params KeyboardButton[] buttons)
    {
        _keyboard.Add(new List<KeyboardButton>(buttons));
        return this;
    }

    public ReplyKeyboardMarkupBuilder SetResizeKeyboard(bool resize)
    {
        _resizeKeyboard = resize;
        return this;
    }

    public ReplyKeyboardMarkupBuilder SetOneTimeKeyboard(bool oneTime)
    {
        _oneTimeKeyboard = oneTime;
        return this;
    }

    public ReplyKeyboardMarkupBuilder SetSelective(bool select)
    {
        _selective = select;
        return this;
    }

    public ReplyKeyboardMarkup Build()
    {
        var markup = new ReplyKeyboardMarkup(_keyboard.ToArray())
        {
            ResizeKeyboard = _resizeKeyboard,
            OneTimeKeyboard = _oneTimeKeyboard,
            Selective = _selective
        };

        return markup;
    }
}

public static class ReplyKeyboardMarkupBuilderExtensions
{
    public static ReplyKeyboardMarkupBuilder AddRow(
        this ReplyKeyboardMarkupBuilder builder,
        params string[] buttonsAsString)
    {
        var keyboardButtons = buttonsAsString
            .Select(p => new KeyboardButton(p))
            .ToArray();

        builder.AddRow(keyboardButtons);
        
        return builder;
    }
}