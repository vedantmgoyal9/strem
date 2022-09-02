﻿using System.ComponentModel.DataAnnotations;
using Strem.Core.Flows.Tasks;
using Strem.Core.Types;

namespace Strem.Flows.Default.Flows.Tasks.Variables;

public class GenerateRandomNumberTaskData : IFlowTaskData
{
    public static readonly string TaskCode = "generate-random-number";
    public static readonly string TaskVersion = "1.0.0";
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code => TaskCode;
    public string Version { get; set; } = TaskVersion;

    [Required]
    public string Name { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
    [Required]
    public VariableScopeType ScopeType { get; set; }

    [Required]
    public string MinimumNumber { get; set; }
    [Required]
    public string MaximumNumber { get; set; }
}