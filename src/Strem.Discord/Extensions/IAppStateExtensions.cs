using Strem.Core.State;
using Strem.Discord.Variables;

namespace Strem.Discord.Extensions;

public static class IAppStateExtensions
{
    public static bool HasDiscordOAuth(this IAppState state) => state.AppVariables.Has(DiscordVars.OAuthToken);
    public static string GetDiscordOAuthToken(this IAppState state) => state.AppVariables.Get(DiscordVars.OAuthToken);
    public static bool HasDiscordScope(this IAppState state, string scope) => state.AppVariables.Get(DiscordVars.OAuthScopes).Contains(scope, StringComparison.OrdinalIgnoreCase);
    public static string[] GetDiscordScopes(this IAppState state) => state.AppVariables.Get(DiscordVars.OAuthScopes).Split(",");
}