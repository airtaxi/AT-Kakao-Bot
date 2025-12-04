using KakaoBotAT.Commons;
using KakaoBotAT.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace KakaoBotAT.Server.Controllers;

[ApiController]
[Route("api/kakao")]
public class KakaoController(IKakaoService kakaoService) : ControllerBase
{
    /// <summary>
    /// Receives notification messages from the MAUI client and returns an immediate response.
    /// POST /api/kakao/notify
    /// </summary>
    [HttpPost("notify")]
    [ProducesResponseType(typeof(ServerResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Notify([FromBody] ServerNotification notification)
    {
        if (notification == null || notification.Data == null)
        {
            return BadRequest(new ServerResponse { Action = "error", Message = "Invalid notification data." });
        }

        var response = await kakaoService.HandleNotificationAsync(notification);
        return Ok(response);
    }

    /// <summary>
    /// Responds to polling requests from the MAUI client by delivering server queued commands.
    /// GET /api/kakao/command
    /// </summary>
    [HttpGet("command")]
    [ProducesResponseType(typeof(ServerResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Command()
    {
        var command = await kakaoService.GetPendingCommandAsync();
        return Ok(command);
    }
}