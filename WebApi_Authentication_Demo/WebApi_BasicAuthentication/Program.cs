using DbServiceLib;
using Microsoft.Extensions.DependencyInjection;

var folder = Environment.CurrentDirectory;
var parentFolder = Directory.GetParent(folder);
Console.WriteLine(parentFolder.FullName);

var builder = WebApplication.CreateBuilder(args);

//使用依赖注入注册服务
builder.Services.AddSingleton<IDbService, DbService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
