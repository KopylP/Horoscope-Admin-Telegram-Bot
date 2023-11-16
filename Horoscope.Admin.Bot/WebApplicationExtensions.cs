using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Models;
using Microsoft.AspNetCore.Mvc;

namespace Horoscope.Admin.Bot;

public static class WebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var botSettings = app.Services.GetRequiredService<BotSettings>();

        app.MapPost($"/bot/{botSettings.EscapedBotToken}", async (
                NewtonsoftJsonUpdate update,
                [FromServices] IChainOfResponsibilityHandler<NewtonsoftJsonUpdate> handler) =>
            {
                if (update.Message is null)
                    return Results.Ok();

                await handler.HandleAsync(update);

                return Results.Ok();
            })
            .WithName("TelegramWebhook");
    }
}