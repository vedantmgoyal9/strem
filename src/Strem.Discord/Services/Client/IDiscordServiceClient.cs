namespace Strem.Discord.Services.Client;

public interface IDiscordServiceClient : IDisposable
{
    bool IsConnected { get; }

    Task Connect();
    void Disconnect();
    
    bool IsMuted();
    void SetMuteStatus(bool shouldMute);
    Task SendMessage(string message, string channelName);
}