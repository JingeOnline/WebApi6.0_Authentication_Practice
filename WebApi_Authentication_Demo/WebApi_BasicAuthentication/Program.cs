using DbServiceLib;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using WebApi_BasicAuthentication.Authentication;

///使用Basic Authentication，服务器端在用户不关闭浏览器的情况下，并没有一种有效的方法来让用户注销。
///用户自己想要注销登录，也只能关闭浏览器。

var builder = WebApplication.CreateBuilder(args);

//添加依赖注入
builder.Services.AddSingleton<IDbService, DbService>();
builder.Services.AddTransient<StudentManagementDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册Basic Authentication服务
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
    ("BasicAuthentication",null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//还需要启用Authentication中间件
app.UseAuthentication();
//---------------------------

app.UseAuthorization();

app.MapControllers();

app.Run();


//回去看看这个教程：https://code-maze.com/net-core-web-development-part6/