using System.Reactive.Disposables;
using Strem.Core.Events.Bus;
using Strem.Core.Extensions;
using Strem.Core.Plugins;
using Strem.Core.State;
using Strem.Discord.Events.OAuth;
using Strem.Discord.Extensions;
using Strem.Discord.Services.Client;

namespace Strem.Discord.Plugins;

public class DiscordPluginStartup : IPluginStartup, IDisposable
{
    private CompositeDisposable _subs = new CompositeDisposable();

    public IEventBus EventBus { get; }
    public IAppState AppState { get; }
    public IDiscordClient Client { get; }
    public ILogger<DiscordPluginStartup> Logger { get; }
    
    public DiscordPluginStartup(IEventBus eventBus, IAppState appState, ILogger<DiscordPluginStartup> logger, IDiscordClient client)
    {
        EventBus = eventBus;
        AppState = appState;
        Logger = logger;
        Client = client;
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
        
        AttemptToConnectClient();
    }

    public void AttemptToConnectClient()
    {
        if(!AppState.HasDiscordOAuth()) { return; }
        if(Client.IsConnected) { return; }
        Client.Connect();
    }

    public void DisconnectEverything()
    {
        if (Client.IsConnected)
        { Client.Disconnect(); }
    }

    public void Dispose() => _subs?.Dispose();
}