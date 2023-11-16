namespace Horoscope.Admin.Bot.Commands;

public static class ReplyCommands
{
    public static class Common
    {
        public const string Back = "\u2b05\ufe0f Назад";
        public const string NavigateBeginning = "Головне меню \ud83d\udd04";
    }

    public static class BeginningHoroscopeEdit
    {
        public const string Manually = "Хочу вручну \u270d\ufe0f";
        public const string LoadFile = "Завантажу файл \ud83d\udce4";
    }
    
    public static class Continue
    {
        public const string NextSign = "Наступний знак \u2652\ufe0f";
        public const string NextDate = "Наступна дата \ud83d\udcc5";
    }

    public static class Preview
    {
        public const string Edit = "Редагувати \u270f\ufe0f";
    }
}