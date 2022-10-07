using Discord;
using Discord.WebSocket;
using DiscordRPC;
using Strem.Core.Extensions;
using Strem.Core.State;
using Strem.Core.Variables;
using Strem.Discord.Extensions;

namespace Strem.Discord.Services.Client;

public class DiscordServiceClient : IDiscordServiceClient
{
    public IApplicationConfig AppConfig { get; }
    public IAppState AppState { get; }
    public ILogger<IDiscordServiceClient> Logger { get; }
    public DiscordSocketClient SocketClient { get; }

    public bool IsConnected { get; private set; }
    
    public DiscordRpcClient RpcClient { get; private set; }

    public DiscordServiceClient(IApplicationConfig appConfig, IDiscordClient socketClient, ILogger<IDiscordServiceClient> logger, IAppState appState)
    {
        AppConfig = appConfig;
        Logger = logger;
        AppState = appState;
        SocketClient = socketClient as DiscordSocketClient;
    }

    public async Task Connect()
    {
        //var clientId = AppConfig.GetDiscordClientId();
        //RpcClient = new DiscordRpcClient(clientId);
        //RpcClient.Initialize();
        var token = AppState.GetDiscordOAuthToken();
        await SocketClient.LoginAsync(TokenType.Bearer, token, false);
        await SocketClient.StartAsync();
        SocketClient.Log += async x => Logger.Information(x.Message); 
        IsConnected = true;
    }

    public void Disconnect()
    {
        RpcClient?.Dispose();
        IsConnected = false;
    }

    public bool IsMuted()
    {
        if (!IsConnected) throw new Exception("Discord is not connected");
        return false;
    }

    public void SetMuteStatus(bool shouldMute)
    {
        if (!IsConnected) throw new Exception("Discord is not connected");
    }

    public async Task SetStatus(string status)
    {
        if (!IsConnected) throw new Exception("Discord is not connected");
    }
    
    public async Task SendMessage(string message, string channelName)
    {
        if (!IsConnected) throw new Exception("Discord is not connected");
        
        var channels = await SocketClient.GetGroupChannelsAsync();
        var groupChannels = SocketClient.GroupChannels;
        var a = await SocketClient.Rest.GetGroupChannelsAsync();
        var channel = channels.SingleOrDefault(x => x.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
        if (channel is null)
        {
            Logger.Warning($"Cant find Discord Channel {channelName}");
            return;
        }

        await channel.SendMessageAsync(message);
    }

    public void Dispose() => Disconnect();
}