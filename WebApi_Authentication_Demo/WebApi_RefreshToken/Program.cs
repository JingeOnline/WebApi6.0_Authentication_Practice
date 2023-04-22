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

            //��AppSettings��Config�ļ��ж�����������ת����AppSetting����
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            //����DataContext�������ַ���������Sqliteֻ��ָ��·�����ɡ�
            builder.Services.AddDbContext<DataContext>(
                    options => options.UseSqlite("Data Source=Database.db"));
            //ע��ӿڵ�����
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