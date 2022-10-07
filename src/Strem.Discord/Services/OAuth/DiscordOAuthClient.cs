using Newtonsoft.Json;
using RestSharp;
using Strem.Core.Events.Bus;
using Strem.Core.Extensions;
using Strem.Core.Services.Browsers.Web;
using Strem.Core.Services.Utils;
using Strem.Core.State;
using Strem.Core.Variables;
using Strem.Discord.Events.OAuth;
using Strem.Discord.Extensions;
using Strem.Discord.Models;
using Strem.Discord.Variables;
using Strem.Infrastructure.Services.Api;

namespace Strem.Discord.Services.OAuth;

public class DiscordOAuthClient : IDiscordOAuthClient
{
    public static readonly string OAuthCallbackUrl = $"http://localhost:{InternalWebHostConfiguration.ApiHostPort}/api/discord/oauth";
    public static readonly string DiscordApiUrl = "https://discord.com/api/oauth2";
    public static readonly string AuthorizeEndpoint = "authorize";
    public static readonly string RevokeEndpoint = "token/revoke";

    public IWebBrowser WebBrowser { get; }
    public IAppState AppState { get; }
    public IEventBus EventBus { get; }
    public IRandomizer Randomizer { get; }
    public IApplicationConfig AppConfig { get; }
    public ILogger<DiscordOAuthClient> Logger { get; }
    
    public DiscordOAuthClient(IWebBrowser webBrowser, IAppState appState, IRandomizer randomizer, IEventBus eventBus,
        ILogger<DiscordOAuthClient> logger, IApplicationConfig appConfig)
    {
        WebBrowser = webBrowser;
        AppState = appState;
        Randomizer = randomizer;
        EventBus = eventBus;
        Logger = logger;
        AppConfig = appConfig;
    }

    public void StartAuthorisationProcess(string[] requiredScopes)
    {
        Logger.Information("Starting Discord OAuth Process");
        
        var state = Randomizer.RandomString(10);
        AppState.TransientVariables.Set(CommonVariables.OAuthState, DiscordVars.Context, state);

        var clientId = AppConfig.GetDiscordClientId();
        var scopes = Uri.EscapeDataString(string.Join(" ", requiredScopes));
        var queryString = $"client_id={clientId}&redirect_uri={OAuthCallbackUrl}&response_type=token&scope={scopes}&state={state}";
        var url = $"{DiscordApiUrl}/{AuthorizeEndpoint}?{queryString}";
        WebBrowser.LoadUrl(url);
    }

    public string AttemptGetAccessToken()
    {
        if (AppState.HasDiscordOAuth()) { return AppState.GetDiscordOAuthToken(); }
        Logger.Error("Cannot find OAuth Token In Vars for request to Twitch OAuth API");
        return string.Empty;
    }

    public void ClearTokenState()
    {
        AppState.AppVariables.Delete(DiscordVars.TokenExpiry);
        AppState.AppVariables.Delete(DiscordVars.OAuthScopes);
        AppState.AppVariables.Delete(DiscordVars.OAuthToken);
    }

    public async Task<bool> RevokeToken()
    {
        Logger.Information("Revoking Discord Token");
        var accessToken = AttemptGetAccessToken();
        if (string.IsNullOrEmpty(accessToken)) { return false; }
        
        var restClient = new RestClient(DiscordApiUrl);
        var restRequest = new RestRequest(RevokeEndpoint, Method.Post);
        restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

        var clientId = AppConfig.GetDiscordClientId();
        var content = $"client_id={clientId}&token={accessToken}";
        restRequest.AddStringBody(content, DataFormat.None);
        
        var response = await restClient.ExecuteAsync(restRequest);
        if (!response.IsSuccessful)
        {
            Logger.Error($"Revoke Error: {(response.Content ?? "unknown error revoking")}");
            ClearTokenState();
            return false;
        }
        
        EventBus.PublishAsync(new DiscordOAuthRevokedEvent());
        ClearTokenState();
        return true;
    }
}