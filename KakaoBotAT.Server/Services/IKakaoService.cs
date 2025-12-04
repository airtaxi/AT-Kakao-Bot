using KakaoBotAT.Commons;

namespace KakaoBotAT.Server.Services;

public interface IKakaoService
{
    /// <summary>
    /// Processes notification messages received from the client and generates an immediate response.
    /// This method is used to respond synchronously to commands (!ping) included in the notification.
    /// </summary>
    /// <param name="notification">The received notification data.</param>
    /// <returns>Action to be performed by the client on KakaoTalk (reply, read, etc.).</returns>
    Task<ServerResponse> HandleNotificationAsync(ServerNotification notification);

    /// <summary>
    /// Retrieves queued commands from the server for the client. (For polling)
    /// This method is used when the server needs to send commands to the client at any time, regardless of notification reception.
    /// </summary>
    /// <returns>Command to execute or empty command.</returns>
    Task<ServerResponse> GetPendingCommandAsync();
}