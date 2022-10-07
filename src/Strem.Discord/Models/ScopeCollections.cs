using Strem.Discord.Types;

namespace Strem.Discord.Models;

public class ScopeCollections
{
    public static string[] ReadChatScopes = new string[1]
    {
        ApiScopes.ReadMessages
    };
    public static string[] ReadVoiceScopes = new string[1]
    {
        ApiScopes.ReadVoiceRpc
    };
    public static string[] ManageVoiceScopes = new string[2]
    {
        ApiScopes.Rpc,
        ApiScopes.WriteVoiceRpc
    };
}