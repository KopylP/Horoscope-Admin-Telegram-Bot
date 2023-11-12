using Google.Cloud.Firestore;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Handlers;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Services;
using Telegram.Bot;

namespace Horoscope.Admin.Bot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChainHandlers(this IServiceCollection services)
    {
        services
            .Chain<IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>>()
            .Add<ExecutionContextSetupHandler>()
            .Add<StartHandler>()
            .Add<AwaitingApiKeyHandler>()
            .Add<BeginningHoroscopeEditHandler>()
            .Add<BackHandler>()
            .Add<AwaitingDateInputHandler>()
            .Add<NavigateBeginningHandler>()
            .Add<AwaitingZodiacSignHandler>()
            .Add<PreviewHandler>()
            .Add<AwaitingForesightInputHandler>()
            .Add<ForesightInputReceivedHandler>()
            .Configure();

        return services;
    }

    public static IServiceCollection AddFirestoreDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var firebaseSettings = configuration.GetRequiredSection("FirebaseSettings").Get<FirebaseSettings>()!;
        
        services.AddSingleton<FirestoreDb>(_ => new FirestoreDbBuilder
        {
            ProjectId = firebaseSettings.ProjectId,
            CredentialsPath = firebaseSettings.JsonCredentials
        }.Build());

        services.AddScoped<FirestoreProvider>();

        return services;
    }

    public static IServiceCollection AddTelegram(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var botSettings = configuration.GetRequiredSection("BotConfiguration").Get<BotSettings>()!;
        services.AddSingleton(botSettings);
        services.AddHttpClient("TelegramWebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botSettings.BotToken!, httpClient));
        services.AddHostedService<ConfigureWebhook>();
        
        return services;
    }
}