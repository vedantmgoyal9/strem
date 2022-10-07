using DiscordRPC;
using Strem.Core.Variables;
using Strem.Discord.Extensions;

namespace Strem.Discord.Services.Client;

public class DiscordClient : IDiscordClient
{
    public IApplicationConfig AppConfig { get; }
    public bool IsConnected { get; private set; }
    public DiscordRpcClient InternalClient { get; private set; }

    public DiscordClient(IApplicationConfig appConfig)
    {
        AppConfig = appConfig;
    }

    public void Connect()
    {
        var clientId = AppConfig.GetDiscordClientId();
        InternalClient = new DiscordRpcClient(clientId);
        IsConnected = true;
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

    public void Disconnect()
    {
        InternalClient?.Dispose();
        IsConnected = false;
    }

    public void Dispose() => Disconnect();
}