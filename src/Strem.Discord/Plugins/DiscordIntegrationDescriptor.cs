using Strem.Core.Services.Registries.Integrations;
using Strem.Core.Variables;
using Strem.Discord.Components.Integrations;

namespace Strem.Discord.Plugins;

public class DiscordIntegrationDescriptor : IIntegrationDescriptor
{
    public string Title => "Discord Integration";

    public string Code => "discord-integration";

    public VariableDescriptor[] VariableOutputs { get; } = Array.Empty<VariableDescriptor>();

    public Type ComponentType { get; } = typeof (DiscordIntegrationComponent);
}