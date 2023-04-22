using Microsoft.EntityFrameworkCore;
using WebApi_RefreshToken.Authorization;
using WebApi_RefreshToken.Helpers;
using WebApi_RefreshToken.Services;

namespace WebApi_RefreshToken
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //把AppSettings从Config文件中读出来，并且转换成AppSetting对象。
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            //设置DataContext的连接字符串，对于Sqlite只需指定路径即可。
            builder.Services.AddDbContext<DataContext>(
                    options => options.UseSqlite("Data Source=Database.db"));
            //注入接口到容器
            builder.Services.AddScoped<IJwtUtils, JwtUtils>();
            builder.Services.AddScoped<IUserService, UserService>();

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

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}