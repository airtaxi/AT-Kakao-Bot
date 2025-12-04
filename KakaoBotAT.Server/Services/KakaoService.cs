using KakaoBotAT.Commons;
using System.Diagnostics;

namespace KakaoBotAT.Server.Services;

/// <summary>
/// Service implementation that handles bot logic.
/// </summary>
public class KakaoService(ILogger<KakaoService> logger) : IKakaoService
{
    /// <summary>
    /// Processes received notifications and checks for !ping command.
    /// </summary>
    public Task<ServerResponse> HandleNotificationAsync(ServerNotification notification)
    {
        var data = notification.Data;

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("[NOTIFY] Received from Room: {RoomName}, Sender: {SenderName}, Content: {Content}", data.RoomName, data.SenderName, data.Content);

        if (data.Content.Trim().Equals("!핑", StringComparison.OrdinalIgnoreCase))
        {
            var response = new ServerResponse
            {
                Action = "send_text",
                RoomId = data.RoomId,
                Message = $"퐁"
            };

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("[RESPONSE] Responding to !핑 command: {Message}", response.Message);

            return Task.FromResult(response);
        }

        return Task.FromResult(new ServerResponse());
    }

    /// <summary>
    /// Retrieves queued commands. (Currently the queued command feature is not implemented.)
    /// </summary>
    public Task<ServerResponse> GetPendingCommandAsync()
    {
        // Currently there is no queued command feature, so return an empty response (to allow the client to process without errors)
        return Task.FromResult(new ServerResponse());
    }
}