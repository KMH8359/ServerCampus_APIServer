using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
// using basic_07.Repository;
// using basic_07.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);       // 웹 애플리케이션 객체를 만들기 위한 빌더 생성

builder.Services.AddLogging();
builder.Services.AddControllers();  // HTTP 컨트롤러 객체 생성

builder.Logging.ClearProviders();
builder.Logging.AddZLoggerConsole();

var app = builder.Build();  // 웹 애플리케이션 객체 생성

app.UseRouting();   // HTTP 요청의 경로를 설정하는 라우팅 활성화


//app.UseMiddleware<HIVESERVER.Middleware.CheckUserAuthAndLoadUserData>();

app.MapDefaultControllerRoute();

IConfiguration configuration = app.Configuration; // appsettings.json 파일에서 설정값 읽기
DBManager.Init(configuration);

app.Run(configuration["ServerAddress"]);
