namespace Strem.Discord.Types;

public class ApiScopes
{
    public static readonly string Rpc = "rpc";
    public static readonly string ReadVoiceRpc = "rpc.voice.read";
    public static readonly string WriteVoiceRpc = "rpc.voice.write";
    public static readonly string ReadMessages = "messages.read";
    public static readonly string ReadDMMessages = "dm_channels.read";
    public static readonly string JoinDMs = "gdm.join";
    public static readonly string ReadActivities = "activities.read";
    public static readonly string WriteActivities = "activities.write";
    public static readonly string Connections = "connections";
    public static readonly string Identify = "identify";
}