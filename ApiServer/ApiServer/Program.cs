using GAMESERVER.Repository;
using GAMESERVER.Services;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);       // 웹 애플리케이션 객체를 만들기 위한 빌더 생성

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddControllers();

SettingLogger();

var app = builder.Build();  // 웹 애플리케이션 객체 생성

app.UseMiddleware<GAMESERVER.Middleware.CheckUserAuthAndLoadUserData>();

app.MapDefaultControllerRoute();

app.Run(configuration["ServerAddress"]);


void SettingLogger()
{
    ILoggingBuilder logging = builder.Logging;
    _ = logging.ClearProviders();

    string fileDir = configuration["logdir"];

    bool exists = Directory.Exists(fileDir);

    if (!exists)
    {
        _ = Directory.CreateDirectory(fileDir);
    }

    _ = logging.AddZLoggerRollingFile(
        options =>
        {
            options.UseJsonFormatter();
            options.FilePathSelector = (timestamp, sequenceNumber) => $"{fileDir}{timestamp.ToLocalTime():yyyy-MM-dd}_{sequenceNumber:000}.log";
            options.RollingInterval = ZLogger.Providers.RollingInterval.Day;
            options.RollingSizeKB = 1024;
        }); 

    _ = logging.AddZLoggerConsole(options =>
    {
        options.UseJsonFormatter(); 
    });

}