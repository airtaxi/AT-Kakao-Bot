# KakaoBotAT

A KakaoTalk bot system built with .NET 10 that uses Android's NotificationListenerService to intercept and respond to KakaoTalk messages.

## Author

**airtaxi**

## License

This project is licensed under the MIT License.

## Overview

KakaoBotAT is a distributed system consisting of three main components:

- **KakaoBotAT.MobileClient**: A .NET MAUI Android application that listens to KakaoTalk notifications and communicates with the server
- **KakaoBotAT.Server**: An ASP.NET Core REST API server that processes messages and sends commands
- **KakaoBotAT.Commons**: Shared data models and contracts used by both client and server

## Architecture

```
┌─────────────────────┐         ┌─────────────────────┐
│  KakaoTalk App      │         │  MAUI Client        │
│  (Android)          │         │  (Android)          │
└──────────┬──────────┘         └──────────┬──────────┘
           │                               │
           │ Notification                  │
           └──────────────────────────────►│
                                           │
                                           │ POST /api/kakao/notify
                                           ▼
                              ┌────────────────────────┐
                              │  ASP.NET Core Server   │
                              │  (REST API)            │
                              └────────────────────────┘
                                           │
                                           │ Command Response
                                           ▼
           ┌───────────────────────────────┘
           │ GET /api/kakao/command (Polling)
           │
           ▼
┌──────────────────────┐
│  MAUI Client         │
│  Sends Reply/Read    │
└──────────────────────┘
```

## Features

### Mobile Client
- **Notification Listener**: Intercepts KakaoTalk notifications using Android's NotificationListenerService
- **Message Processing**: Extracts message content, sender information, and room details
- **Action Execution**: Can send replies and mark messages as read
- **Server Communication**: Sends notifications to server and polls for commands
- **Battery Optimization**: Implements WakeLock to ensure continuous operation
- **Permission Management**: Guides users through notification access and battery optimization settings

### Server
- **REST API**: Provides endpoints for receiving notifications and delivering commands
- **Command Processing**: Handles bot commands (e.g., `!핑` command responds with `퐁`)
- **Extensible Design**: Easy to add new bot commands and features
- **Logging**: Built-in logging for debugging and monitoring

## Technology Stack

- **.NET 10**: Latest .NET framework
- **C# 14.0**: Latest C# language features
- **.NET MAUI**: Cross-platform UI framework (Android target)
- **ASP.NET Core**: Web API framework
- **CommunityToolkit.Mvvm**: MVVM helpers and patterns
- **System.Text.Json**: JSON serialization

## Prerequisites

### For Development
- Visual Studio 2022 or later
- .NET 10 SDK
- Android SDK (API Level 21+)
- Android device or emulator with KakaoTalk installed

### For Deployment
- **Mobile Client**: Android device with KakaoTalk installed
- **Server**: Any platform supporting .NET 10 (Windows, Linux, macOS)

## Setup

### 1. Clone the Repository
```bash
git clone https://github.com/airtaxi/KakaoBotAT.git
cd KakaoBotAT
```

### 2. Configure Server Endpoint
Edit `KakaoBotAT.MobileClient\Constants.cs` and update the server URL:
```csharp
internal const string ServerEndpointUrl = "https://your-server-url.com/api/kakao";
```

### 3. Build the Solution
```bash
dotnet build
```

### 4. Run the Server
```bash
cd KakaoBotAT.Server
dotnet run
```

### 5. Deploy Mobile Client
Deploy the `KakaoBotAT.MobileClient` project to your Android device through Visual Studio.

## Usage

### Mobile Client Setup

1. **Grant Notification Access**
   - Open the app
   - Tap "Open Notification Listener Settings"
   - Enable notification access for KakaoBotAT

2. **Disable Battery Optimization**
   - Tap "Request Battery Optimization Exemption"
   - Allow the app to run in the background

3. **Configure Server**
   - Enter your server endpoint URL
   - Tap "Update Status" to verify settings

4. **Start the Bot**
   - Tap "Start Bot"
   - The app will now listen for KakaoTalk messages and communicate with the server

### Adding Bot Commands

Edit `KakaoBotAT.Server\Services\KakaoService.cs` to add new commands:

```csharp
public Task<ServerResponse> HandleNotificationAsync(ServerNotification notification)
{
    var data = notification.Data;
    
    // Example: Add a new command
    if (data.Content.Trim().Equals("!hello", StringComparison.OrdinalIgnoreCase))
    {
        return Task.FromResult(new ServerResponse
        {
            Action = "send_text",
            RoomId = data.RoomId,
            Message = "Hello! How can I help you?"
        });
    }
    
    // Existing commands...
}
```

## API Endpoints

### POST /api/kakao/notify
Receives notification messages from the MAUI client.

**Request Body:**
```json
{
  "event": "message",
  "data": {
    "roomName": "Chat Room",
    "roomId": "room_id_hash",
    "senderName": "Sender Name",
    "senderHash": "sender_hash",
    "content": "Message content",
    "logId": "123456",
    "isGroupChat": false,
    "time": 1234567890
  }
}
```

**Response:**
```json
{
  "action": "send_text",
  "roomId": "room_id_hash",
  "message": "Bot response"
}
```

### GET /api/kakao/command
Polling endpoint for retrieving queued commands.

**Response:**
```json
{
  "action": "send_text",
  "roomId": "room_id_hash",
  "message": "Command message"
}
```

## Project Structure

```
KakaoBotAT/
├── KakaoBotAT.Commons/
│   ├── KakaoMessageData.cs        # Message data model
│   ├── ServerNotification.cs      # Notification request model
│   └── ServerResponse.cs          # Server response model
├── KakaoBotAT.MobileClient/
│   ├── Platforms/
│   │   └── Android/
│   │       ├── KakaoNotificationListener.cs  # Notification interceptor
│   │       └── KakaoBotService.cs            # Android-specific services
│   ├── ViewModels/
│   │   └── MainViewModel.cs       # Main UI logic
│   ├── MainPage.xaml              # Main UI
│   ├── Constants.cs               # Configuration constants
│   └── IKakaoBotService.cs        # Service interface
├── KakaoBotAT.Server/
│   ├── Controllers/
│   │   └── KakaoController.cs     # API endpoints
│   ├── Services/
│   │   ├── IKakaoService.cs       # Service interface
│   │   └── KakaoService.cs        # Bot logic implementation
│   └── Program.cs                 # Server entry point
└── README.md
```

## Permissions Required

### Android Permissions
- `INTERNET`: Network communication
- `ACCESS_NETWORK_STATE`: Check network connectivity
- `BIND_NOTIFICATION_LISTENER_SERVICE`: Listen to notifications
- `REQUEST_IGNORE_BATTERY_OPTIMIZATIONS`: Background operation
- `WAKE_LOCK`: Keep CPU awake

## Troubleshooting

### Bot Not Receiving Messages
1. Verify notification access is granted
2. Check battery optimization is disabled
3. Ensure KakaoTalk is installed and logged in
4. Verify server endpoint URL is correct

### Server Connection Issues
1. Check server is running and accessible
2. Verify firewall settings
3. Check server endpoint URL in Constants.cs
4. Review network connectivity on mobile device

### Replies Not Sending
1. Reply actions may expire if notification is dismissed
2. Ensure KakaoTalk notification is still visible
3. Check logs for detailed error messages

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## Disclaimer

This project is for educational purposes. Make sure to comply with KakaoTalk's Terms of Service when using this bot.

## MIT License

```
MIT License

Copyright (c) 2024 airtaxi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
