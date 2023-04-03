using DbServiceLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi_JwtAuthentication.Authentication;
using WebApi_JwtAuthentication.Models;
//using ConfigurationManager = WebApi_JwtAuthentication.Helpers.ConfigurationManager;


//相关教程：https://www.c-sharpcorner.com/article/jwt-token-authentication-using-the-net-core-6-web-api/
//https://jasonwatmore.com/post/2021/12/14/net-6-jwt-authentication-tutorial-with-example-api
//从appsetting中获取键值对，可以参考这个 https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html

//发送登录请求的时候[POST]http://localhost:5000/api/user
//在Postman中测试的时候：Body中选择raw，格式选择JSON，然后发送用户名和密码即可。

//在访问需要验证的页面时，PostMan中Auth选择Bearer Token，然后把Token复制进去即可。

var builder = WebApplication.CreateBuilder(args);

//添加依赖注入
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<StudentManagementDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//从appsettings.json文件中，读取储存的对象和值
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//配置Swagger
builder.Services.AddSwaggerGen(options =>
{
        options.SwaggerDoc("V1", new OpenApiInfo
        {
            Version = "V1",
            Title = "WebAPI JWT Authentication",
            Description = "Product WebAPI"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Bearer Authentication with JWT Token",
            Type = SecuritySchemeType.Http
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
        });
});

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = ConfigurationManager.AppSetting["JWT:ValidIssuer"],
        //ValidAudience = ConfigurationManager.AppSetting["JWT:ValidAudience"],

        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        // 否则，当生命周期小于5分钟的时候，生命周期不起作用，直到5分钟后才会失效。
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:SecretKey"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.在配置完Swagger后，需要手动把它添加到pipeline中。
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Testing WebAPI");
    });
}


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

