using Strem.Core.Events.Bus;
using Strem.Core.State;
using Strem.Core.Variables;
using Strem.Discord.Extensions;
using Strem.Discord.Services.Client;
using Strem.Flows.Data.Tasks;
using Strem.Flows.Executors;
using Strem.Flows.Processors;

namespace Strem.Discord.Flows.Tasks;

public class SetDiscordStatusTask : FlowTask<SetDiscordStatusTaskData>
{
    public override string Code => SetDiscordStatusTaskData.TaskCode;
    public override string Version => SetDiscordStatusTaskData.TaskVersion;
    
    public override string Name => "Set Discord Status";
    public override string Category => "Discord";
    public override string Description => "Sets the discord status";

    public IDiscordServiceClient DiscordServiceClient { get; }

    public SetDiscordStatusTask(ILogger<FlowTask<SetDiscordStatusTaskData>> logger, IFlowStringProcessor flowStringProcessor, IAppState appState, IEventBus eventBus, IDiscordServiceClient discordServiceClient) : base(logger, flowStringProcessor, appState, eventBus)
    {
        DiscordServiceClient = discordServiceClient;
    }

    public override bool CanExecute() => AppState.HasDiscordOAuth() && DiscordServiceClient.IsConnected;

    public override async Task<ExecutionResult> Execute(SetDiscordStatusTaskData data, IVariables flowVars)
    {
        var processedStatus = FlowStringProcessor.Process(data.NewStatus, flowVars);
        
        return ExecutionResult.Success();
    }
}