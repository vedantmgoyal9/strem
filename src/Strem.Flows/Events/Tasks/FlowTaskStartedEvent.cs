﻿using Strem.Flows.Events.Base;

namespace Strem.Flows.Events.Tasks;

public record FlowTaskStartedEvent(Guid FlowId, Guid TaskId) : FlowTaskEvent(FlowId, TaskId);