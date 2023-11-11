using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Horoscope.Admin.Bot.Services;
public sealed class ConfigureWebhook : IHostedService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _services;
    private readonly BotSettings _botConfig;

    public ConfigureWebhook(
        ILogger<ConfigureWebhook> logger,
        IServiceProvider serviceProvider,
        BotSettings botConfig)
    {
        _logger = logger;
        _services = serviceProvider;
        _botConfig = botConfig;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Configure custom endpoint per Telegram API recommendations:
        // https://core.telegram.org/bots/api#setwebhook
        // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
        // using a secret path in the URL, e.g. https://www.example.com/<token>.
        // Since nobody else knows your bot's token, you can be pretty sure it's us.
        var webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.EscapedBotToken}";

        _logger.LogInformation("Setting webhook: {webhookAddress}", webhookAddress);

        await botClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Remove webhook upon app shutdown
        _logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}