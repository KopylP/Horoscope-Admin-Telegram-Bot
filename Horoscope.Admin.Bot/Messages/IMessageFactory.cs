using Horoscope.Admin.Bot.Framework.Messages;

namespace Horoscope.Admin.Bot.Messages;

public interface IMessageFactory
{
    public IMessage CreateStandardMessage(string message);
}