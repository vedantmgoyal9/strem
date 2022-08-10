﻿using Strem.Core.Events.Bus;
using Strem.Core.Extensions;
using Strem.Core.Flows;
using Strem.Core.Flows.Executors;
using Strem.Core.Flows.Processors;
using Strem.Core.Flows.Tasks;
using Strem.Core.State;
using Strem.Core.Variables;
using Strem.Twitch.Extensions;
using TwitchLib.Client.Interfaces;

namespace Strem.Twitch.Flows.Tasks.Chat;

public class SendTwitchWhisperMessageTask : FlowTask<SendTwitchWhisperMessageTaskData>
{
    public override string Code => SendTwitchWhisperMessageTaskData.TaskCode;
    public override string Version => SendTwitchWhisperMessageTaskData.TaskVersion;
    
    public override string Name => "Send Twitch Whisper Message";
    public override string Category => "Twitch";
    public override string Description => "Sends a whisper to a user on twitch";

    public ITwitchClient TwitchClient { get; }

    public SendTwitchWhisperMessageTask(ILogger<FlowTask<SendTwitchWhisperMessageTaskData>> logger, IFlowStringProcessor flowStringProcessor, IAppState appState, IEventBus eventBus, ITwitchClient twitchClient) : base(logger, flowStringProcessor, appState, eventBus)
    {
        TwitchClient = twitchClient;
    }

    public override bool CanExecute() => AppState.HasTwitchOAuth();

    public override async Task Execute(SendTwitchWhisperMessageTaskData data, IVariables flowVars)
    {
        var processedMessage = FlowStringProcessor.Process(data.Message, flowVars);
        TwitchClient.SendWhisper(data.Username, processedMessage);
    }
}