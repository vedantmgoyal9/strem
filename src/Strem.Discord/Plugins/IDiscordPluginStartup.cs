using System.Reactive.Disposables;
using Strem.Core.Events.Bus;
using Strem.Core.Plugins;
using Strem.Core.State;

namespace Strem.Discord.Plugins;

public class DiscordPluginStartup : IPluginStartup, IDisposable
{
    private CompositeDisposable _subs = new CompositeDisposable();

    public IEventBus EventBus { get; }
    public IAppState AppState { get; }
    public ILogger<DiscordPluginStartup> Logger { get; }
    
    public DiscordPluginStartup(IEventBus eventBus, IAppState appState, ILogger<DiscordPluginStartup> logger)
    {
        EventBus = eventBus;
        AppState = appState;
        Logger = logger;
    }

    public string[] RequiredConfigurationKeys { get; } = new[] { DiscordPluginSettings.DiscordClientIdKey };
    public Task SetupPlugin() => Task.CompletedTask;

    public async Task StartPlugin()
    {
  
    }

    private void CheckForDiscordUpdates()
    {

    }

    public void Dispose() => _subs?.Dispose();
}