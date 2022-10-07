using Strem.Core.Variables;

namespace Strem.Discord.Variables;

public class DiscordVars
{
    public static readonly string Context = "discord";
    
    public static readonly VariableEntry OAuthToken = new(CommonVariables.OAuthAccessToken, DiscordVars.Context);
    public static readonly VariableEntry TokenExpiry = new("token-expiry", DiscordVars.Context);
    public static readonly VariableEntry OAuthScopes = new("oauth-scopes", DiscordVars.Context);
}