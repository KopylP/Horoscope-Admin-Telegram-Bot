using System.Reflection;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Models;

public class NewtonsoftJsonUpdate : Update
{
    public static async ValueTask<NewtonsoftJsonUpdate?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        using var streamReader = new StreamReader(context.Request.Body);
        var updateJsonString = await streamReader.ReadToEndAsync();

        return JsonConvert.DeserializeObject<NewtonsoftJsonUpdate>(updateJsonString);
    }
}