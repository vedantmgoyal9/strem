using Strem.Core.State;
using Strem.Core.Variables;
using Strem.Discord.Plugins;

namespace Strem.Discord.Extensions;

public static class ApplicationConfigExtensions
{
    public static string GetTwitchClientId(this IApplicationConfig config) => config[DiscordPluginSettings.DiscordClientIdKey].ToString();
}