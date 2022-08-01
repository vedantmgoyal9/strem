﻿namespace Strem.Core.Flows;

public interface IFlowElement
{
    string Version { get; }
    string Code { get; }
    string Name { get; }
    string Description { get; }
}