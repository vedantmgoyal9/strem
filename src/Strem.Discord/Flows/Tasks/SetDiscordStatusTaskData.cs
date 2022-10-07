using System.ComponentModel.DataAnnotations;
using Strem.Flows.Data.Tasks;

namespace Strem.Discord.Flows.Tasks;

public class SetDiscordStatusTaskData : IFlowTaskData
{
    public static readonly string TaskCode = "set-discord-status";
    public static readonly string TaskVersion = "1.0.0";
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code => TaskCode;
    public string Version { get; set; } = TaskVersion;
    
    [Required]
    public string NewStatus { get; set; }
}