using _21_MVC_API.Context;
using _21_MVC_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

//Add Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Identity

builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = false;
    opt.SignIn.RequireConfirmedPhoneNumber = false;
    opt.SignIn.RequireConfirmedAccount = false;

    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//JWT

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["secretKey"];

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        //ValidateLifetime = true,
        //ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//swagger üzerinden login iþlemi için.
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
    {
        Name = "authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "jwt",
        In = ParameterLocation.Header,
        Description = "bearer þemasýný kullanan jwt yetkilendirme baþlýðý.\r\n\r\n aþaðýdaki metin giriþine 'bearer' [boþluk] ve ardýndan üretilen tokený girin.\r\n\r\nörnek: \"bearer 1safsfsdfdfd\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
           new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "bearer"
                 }
             },
             new string[] {}
        }
    });
});

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthDemo v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
