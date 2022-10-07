using Newtonsoft.Json;

namespace Strem.Discord.Models;

public class DiscordOAuthValidationPayload
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }

    public string Login { get; set; }

    public string[] Scopes { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}