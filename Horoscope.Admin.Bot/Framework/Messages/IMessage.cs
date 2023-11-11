namespace Horoscope.Admin.Bot.Framework.Messages;

public interface IMessage
{
    public Task SendAsync();
}