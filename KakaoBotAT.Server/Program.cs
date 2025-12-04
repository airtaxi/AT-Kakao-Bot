using KakaoBotAT.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IKakaoService, KakaoService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();