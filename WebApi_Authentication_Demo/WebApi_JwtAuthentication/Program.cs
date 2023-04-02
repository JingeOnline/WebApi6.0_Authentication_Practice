using DbServiceLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi_JwtAuthentication.Authentication;
using WebApi_JwtAuthentication.Models;
using ConfigurationManager = WebApi_JwtAuthentication.Helpers.ConfigurationManager;


//��ؽ̳̣�https://www.c-sharpcorner.com/article/jwt-token-authentication-using-the-net-core-6-web-api/
//https://jasonwatmore.com/post/2021/12/14/net-6-jwt-authentication-tutorial-with-example-api
//��appsetting�л�ȡ��ֵ�ԣ����Բο���� https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html


var builder = WebApplication.CreateBuilder(args);
//�����Ŀ

//�������ע��
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<StudentManagementDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//��appsettings.json�ļ��У���ȡ����Ķ����ֵ
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//����Swagger
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JwtSetting:SecretKey"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Product WebAPI");
    });
}


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

