using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Keyword;

namespace Horoscope.Admin.Bot.Messages.Extensions;

public static class ReplyKeyboardMarkupExtensions
{
    public static ReplyKeyboardMarkupBuilder AddBackAndCancelRow(this ReplyKeyboardMarkupBuilder builder)
    {
        builder.AddRow(ReplyCommands.Common.Back, ReplyCommands.Common.NavigateBeginning);
        return builder;
    }
    
    public static ReplyKeyboardMarkupBuilder AddBackRow(this ReplyKeyboardMarkupBuilder builder)
    {
        builder.AddRow(ReplyCommands.Common.Back);
        return builder;
    }
}