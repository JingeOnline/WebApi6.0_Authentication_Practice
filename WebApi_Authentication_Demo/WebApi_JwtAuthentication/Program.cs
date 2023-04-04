using DbServiceLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using WebApi_JwtAuthentication.Authentication;
using WebApi_JwtAuthentication.Models;
//using ConfigurationManager = WebApi_JwtAuthentication.Helpers.ConfigurationManager;


//��ؽ̳̣�https://www.c-sharpcorner.com/article/jwt-token-authentication-using-the-net-core-6-web-api/
//https://jasonwatmore.com/post/2021/12/14/net-6-jwt-authentication-tutorial-with-example-api
//��appsetting�л�ȡ��ֵ�ԣ����Բο���� https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html

//���͵�¼�����ʱ��[POST]http://localhost:5000/api/user
//��Postman�в��Ե�ʱ��Body��ѡ��raw����ʽѡ��JSON��Ȼ�����û��������뼴�ɡ�

//�ڷ�����Ҫ��֤��ҳ��ʱ��PostMan��Authѡ��Bearer Token��Ȼ���Token���ƽ�ȥ���ɡ�

var builder = WebApplication.CreateBuilder(args);

//�������ע��
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<StudentManagementDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//��appsettings.json�ļ��У���ȡ����Ķ����ֵ����Կ����С����Ϊ128bit��16Bytes��
//�Ѹ��Զ���������ת���ɶ���Ȼ����ӵ�����ע���С�
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
            Description = "WebAPI JWT Authentication Description"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT Bearer Token SHA-256", //����ֶβ�֪����ʾ������
            In = ParameterLocation.Header,  //��������ʱ��API��Կ��λ��
            Name = "Authorization",  //������Կ�Ĳ�����
            Description = "Bearer Authentication with JWT Token using SHA-256", //��ʾ��������Ϣ
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

//����JWT��֤�м��
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //����JWT��֤
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = ConfigurationManager.AppSetting["JWT:ValidIssuer"],
        //ValidAudience = ConfigurationManager.AppSetting["JWT:ValidAudience"],

        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        // ���򣬵���������С��5���ӵ�ʱ���������ڲ������ã�ֱ��5���Ӻ�Ż�ʧЧ��
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:SecretKey"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.��������Swagger����Ҫ�ֶ�������ӵ�pipeline�С�
if (app.Environment.IsDevelopment()) //��仰�ڵ�ǰ��Ŀ��ûʲô��
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Testing WebAPI");
    });
}

//�����֤�м��
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

