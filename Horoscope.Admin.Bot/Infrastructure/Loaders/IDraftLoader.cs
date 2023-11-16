using Horoscope.Admin.Bot.Models;

namespace Horoscope.Admin.Bot.Infrastructure.Loaders;

public interface IDraftLoader
{
    IEnumerable<Draft> Load(byte[] document);
}