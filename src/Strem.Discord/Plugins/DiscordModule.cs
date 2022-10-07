using Microsoft.Extensions.DependencyInjection;
using Strem.Core.Plugins;
using Strem.Core.Services.Registries.Integrations;
using Strem.Discord.Services.Client;
using Strem.Discord.Services.OAuth;
using Strem.Flows.Extensions;
using Strem.Infrastructure.Services.Api;

namespace Strem.Discord.Plugins;

public class DiscordModule : IRequiresApiHostingModule
{
    public void Setup(IServiceCollection services)
    {
        services.AddSingleton<IIntegrationDescriptor, DiscordIntegrationDescriptor>();
        
        services.AddSingleton<IPluginStartup, DiscordPluginStartup>();
        services.AddSingleton<IDiscordOAuthClient, DiscordOAuthClient>();
        services.AddSingleton<IDiscordClient, DiscordClient>();
        
        var assembly = GetType().Assembly;
        services.RegisterAllTasksAndComponentsIn(assembly);
        services.RegisterAllTriggersAndComponentsIn(assembly);
    }
}