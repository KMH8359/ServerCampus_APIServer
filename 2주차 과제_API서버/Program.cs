
var builder = WebApplication.CreateBuilder(args);       // 웹 애플리케이션 객체를 만들기 위한 빌더 생성

builder.Services.AddControllers();  // HTTP 컨트롤러 객체 생성

var app = builder.Build();  // 웹 애플리케이션 객체 생성

app.UseRouting();   // HTTP 요청의 경로를 설정하는 라우팅 활성화

app.UseEndpoints(endpoints => { endpoints.MapControllers(); }); // 엔드포인트(실행 가능 요청 처리 코드의 단위) 사용 

IConfiguration configuration = app.Configuration; // appsettings.json 파일에서 설정값 읽기
DBManager.Init(configuration);

app.Run(configuration["ServerAddress"]);
