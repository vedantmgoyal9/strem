using Strem.Discord.Types;

namespace Strem.Discord.Models;

public class ScopeCollections
{
    public static string[] ManageChatScopes = new []
    {
        ApiScopes.JoinDMs
    };
    public static string[] ReadChatScopes = new []
    {
        ApiScopes.ReadMessages
    };
    public static string[] ReadVoiceScopes = new []
    {
        ApiScopes.ReadVoiceRpc
    };
    public static string[] ManageVoiceScopes = new []
    {
        ApiScopes.Rpc,
        ApiScopes.WriteVoiceRpc
    };
}