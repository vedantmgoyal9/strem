namespace Strem.Discord.Services.Client;

public interface IDiscordClient : IDisposable
{
    bool IsConnected { get; }

    void Connect();
    void Disconnect();
    
    bool IsMuted();
    void SetMuteStatus(bool shouldMute);
}