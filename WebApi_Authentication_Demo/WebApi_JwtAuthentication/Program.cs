using DbServiceLib;

var builder = WebApplication.CreateBuilder(args);
//这个项目

//添加依赖注入
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<StudentManagementDbContext>();
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
