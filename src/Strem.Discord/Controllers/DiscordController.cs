using Microsoft.AspNetCore.Mvc;
using Strem.Core.Events;
using Strem.Core.Extensions;
using Strem.Core.State;
using Strem.Core.Variables;
using Strem.Discord.Events.OAuth;
using Strem.Discord.Models;
using Strem.Discord.Variables;
using Strem.Infrastructure.Extensions;

namespace Strem.Discord.Controllers;

[ApiController]
[Route("api/discord")]
public class DiscordController : Controller
{
  public IAppState AppState { get; }

  public DiscordController() => this.AppState = WebHostHackExtensions.GetService<IAppState>((ControllerBase) this);

  [HttpGet]
  public IActionResult Index() => Ok("Discord Controller Working Fine");

  [HttpGet]
  [Route("oauth")]
  public IActionResult OAuthCallback() => View("OAuthClient");

  [HttpPost]
  [Route("oauth")]
  public IActionResult OAuthLocalCallback(DiscordOAuthClientPayload payload)
  {
    if (string.IsNullOrEmpty(payload?.AccessToken))
    {
      var errorEvent = new ErrorEvent("Discord Client Callback OAuth", "Error with clientside oauth extraction");
      this.PublishAsyncEvent(errorEvent);
      return BadRequest($"Discord couldn't complete OAuth: {errorEvent.Message}");
    }
    
    var state = AppState.TransientVariables.Get(CommonVariables.OAuthState, DiscordVars.Context);
    if (payload.State != state)
    {
      var errorEvent = new ErrorEvent("Discord Client Callback OAuth", "OAuth state does not match request state");
      this.PublishAsyncEvent(errorEvent);
      return BadRequest($"Discord couldn't complete OAuth: {errorEvent.Message}");
    }
    
    AppState.AppVariables.Set(DiscordVars.OAuthToken, payload.AccessToken);
    var expiry = DateTime.Now.AddSeconds(int.Parse(payload.ExpiresIn));
    AppState.AppVariables.Set(DiscordVars.TokenExpiry, expiry.ToString("u"));
    AppState.AppVariables.Set(DiscordVars.OAuthScopes, payload.Scope.Replace(" ", ","));
    
    this.PublishAsyncEvent(new DiscordOAuthSuccessEvent());
    return Ok("Punch It Chewie!");
  }
}