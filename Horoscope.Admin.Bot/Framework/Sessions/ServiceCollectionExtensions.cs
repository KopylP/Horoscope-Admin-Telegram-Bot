using System.Runtime.CompilerServices;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Sessions.Implementations;

namespace Horoscope.Admin.Bot.Framework.Sessions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessionState<TState, TTrigger>(
        this IServiceCollection services,
        Type assemblyMarker)
    {
        var interfaceType = typeof(ISessionStateConfigurator<TState, TTrigger>);
        var configurators =
            assemblyMarker.Assembly.FindAllImplementations(interfaceType);
        
        foreach (var configurator in configurators)
        {
            services.AddScoped(interfaceType, configurator);
        }

        services.AddScoped<ISessionStateFactory<TState, TTrigger>, SessionStateFactory<TState, TTrigger>>();

        return services;
    }
}