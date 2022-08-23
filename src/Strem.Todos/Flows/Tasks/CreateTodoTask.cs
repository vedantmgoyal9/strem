﻿using Microsoft.Extensions.Logging;
using Strem.Core.Events.Bus;
using Strem.Core.Extensions;
using Strem.Core.Flows.Executors;
using Strem.Core.Flows.Processors;
using Strem.Core.Flows.Tasks;
using Strem.Core.State;
using Strem.Core.Types;
using Strem.Core.Variables;
using Strem.Todos.Data;
using Strem.Todos.Events;

namespace Strem.Todos.Flows.Tasks;

public class CreateTodoTask : FlowTask<CreateTodoTaskData>
{
    public override string Code => CreateTodoTaskData.TaskCode;
    public override string Version => CreateTodoTaskData.TaskVersion;
    
    public override string Name => "Create A Todo";
    public override string Category => "Todo";
    public override string Description => "Creates a todo action within the todo list";

    public ITodoStore TodoStore { get; }

    public CreateTodoTask(ILogger<FlowTask<CreateTodoTaskData>> logger, IFlowStringProcessor flowStringProcessor, IAppState appState, IEventBus eventBus, ITodoStore todoStore) : base(logger, flowStringProcessor, appState, eventBus)
    {
        TodoStore = todoStore;
    }

    public override bool CanExecute() => true;

    private TodoData CreateTodoItem(CreateTodoTaskData data, IVariables flowVars)
    {
        if (!FlowStringProcessor.TryProcessInt(data.TimeoutValue, flowVars, out var timeoutValue))
        {
            Logger.LogWarning($"Unable to process {data.TimeoutValue} into a number, verify it is a number or variables exist");
            timeoutValue = 1;
        }
        
        var timeoutTimespan = data.TimeoutType.ToTimeSpan(timeoutValue);
        var expiryDate = DateTime.Now + timeoutTimespan;
        return new TodoData
        {
            Payload = FlowStringProcessor.Process(data.Payload, flowVars),
            ActionType = data.ActionType,
            Title = FlowStringProcessor.Process(data.Title, flowVars),
            CreatedDate = DateTime.Now,
            ExpiryDate = expiryDate,
            CreatedBy = FlowStringProcessor.Process(data.CreatedBy, flowVars)
        };
    }
    
    public override async Task<ExecutionResult> Execute(CreateTodoTaskData data, IVariables flowVars)
    {
        var newTodoItem = CreateTodoItem(data, flowVars);
        TodoStore.Todos.Add(newTodoItem);
        EventBus.PublishAsync(new TodoCreatedEvent { TodoId = newTodoItem.Id });
        return ExecutionResult.Success();
    }
}