using Google.Cloud.Firestore;
using Horoscope.Admin.Bot;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Handlers;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Repositories;
using Horoscope.Admin.Bot.Services;
using Horoscope.Admin.Bot.Session;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<ConfigureWebhook>();

var botSettings = builder.Configuration.GetRequiredSection("BotConfiguration").Get<BotSettings>()!;
var firebaseSettings = builder.Configuration.GetRequiredSection("FirebaseSettings").Get<FirebaseSettings>()!;

builder.Services.AddSingleton(botSettings);
builder.Services.AddScoped<IMessageFactory, MessageFactory>();
builder.Services.AddScoped<ISessionStateProvider, SessionStateProvider>();
builder.Services.AddScoped<IDraftRepository, DraftRepository>();

builder.Services.AddSingleton<FirestoreDb>(_ => new FirestoreDbBuilder
{
    ProjectId = firebaseSettings.ProjectId,
    CredentialsPath = firebaseSettings.JsonCredentials
}.Build());

builder.Services.AddScoped<FirestoreProvider>();
builder.Services.AddSessionState<State, Trigger>(assemblyMarker: typeof(Program));

builder.Services.AddHttpClient("TelegramWebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botSettings.BotToken!, httpClient));

builder.Services
    .Chain<IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>>()
    .Add<ExecutionContextSetupHandler>()
    .Add<StartHandler>()
    .Add<ApiKeyVerificationHandler>()
    .Add<StartingEditHandler>()
    .Add<BackHandler>()
    .Add<WaitingForDateHandler>()
    .Add<CancelHandler>()
    .Add<WaitingForSignHandler>()
    .Add<PreviewHandler>()
    .Add<WaitingForForecastHandler>()
    .Add<ForecastProvidedHandler>()
    .Configure();

var app = builder.Build();

app.MapPost($"/bot/{botSettings.EscapedBotToken}", async (
        NewtonsoftJsonUpdate update,
        [FromServices] IChainOfResponsibilityHandler<NewtonsoftJsonUpdate> handler) =>
    {
        if (string.IsNullOrWhiteSpace(update.GetMessage()))
            return Results.Ok();

        await handler.HandleAsync(update);

        return Results.Ok();
    })
    .WithName("TelegramWebhook");

app.Run();