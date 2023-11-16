using Horoscope.Admin.Bot;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Infrastructure.Loaders;
using Horoscope.Admin.Bot.Infrastructure.Repositories;
using Horoscope.Admin.Bot.Infrastructure.Utils;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Session;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageFactory, MessageFactory>();
builder.Services.AddScoped<ISessionStateProvider, SessionStateProvider>();
builder.Services.AddScoped<IDraftRepository, DraftRepository>();
builder.Services.AddScoped<IDraftLoader, DraftLoader>();
builder.Services.AddScoped<TelegramFileDownloader>();
builder.Services.AddFirestoreDatabase(builder.Configuration);
builder.Services.AddSessionState<State, Trigger>(assemblyMarker: typeof(Program));
builder.Services.AddChainHandlers();
builder.Services.AddTelegram(builder.Configuration);

var app = builder.Build();

app.MapEndpoints();
app.Run();