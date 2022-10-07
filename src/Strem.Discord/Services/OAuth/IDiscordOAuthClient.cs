namespace Strem.Discord.Services.OAuth;

public interface IDiscordOAuthClient
{
    void StartAuthorisationProcess(string[] requiredScopes);
    Task<bool> RevokeToken();
}