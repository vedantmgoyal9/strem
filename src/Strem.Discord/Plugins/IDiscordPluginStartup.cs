using System.Reactive.Disposables;
using Strem.Core.Events.Bus;
using Strem.Core.Extensions;
using Strem.Core.Plugins;
using Strem.Core.State;
using Strem.Discord.Events.OAuth;
using Strem.Discord.Extensions;
using Strem.Discord.Services.Client;
using Strem.Discord.Services.OAuth;

namespace Strem.Discord.Plugins;

public class DiscordPluginStartup : IPluginStartup, IDisposable
{
    private CompositeDisposable _subs = new();

    public IEventBus EventBus { get; }
    public IAppState AppState { get; }
    public IDiscordServiceClient ServiceClient { get; }
    public IDiscordOAuthClient OAuthClient { get; }
    public ILogger<DiscordPluginStartup> Logger { get; }
    
    public DiscordPluginStartup(IEventBus eventBus, IAppState appState, ILogger<DiscordPluginStartup> logger, IDiscordServiceClient serviceClient, IDiscordOAuthClient oAuthClient)
    {
        EventBus = eventBus;
        AppState = appState;
        Logger = logger;
        ServiceClient = serviceClient;
        OAuthClient = oAuthClient;
    }

    public string[] RequiredConfigurationKeys { get; } = new[] { DiscordPluginSettings.DiscordClientIdKey };
    public Task SetupPlugin() => Task.CompletedTask;

    public async Task StartPlugin()
    {
        EventBus.Receive<DiscordOAuthSuccessEvent>()
            .Subscribe(_ => AttemptToConnectClient())
            .AddTo(_subs);
        
        EventBus.Receive<DiscordOAuthRevokedEvent>()
            .Subscribe(_ => DisconnectEverything())
            .AddTo(_subs);
        
        await AttemptToConnectClient();
    }

    public async Task AttemptToConnectClient()
    {
        if(!AppState.HasDiscordOAuth()) { return; }
        if(ServiceClient.IsConnected) { return; }

        try
        {
            ServiceClient.Connect();
        }
        catch (Exception e)
        {
            Logger.Error($"Unable to connect to Discord: {e.Message}");
        }
    }

    public void DisconnectEverything()
    {
        if (ServiceClient.IsConnected)
        { ServiceClient.Disconnect(); }
    }

    public void Dispose() => _subs?.Dispose();
}