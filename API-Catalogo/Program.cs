using API_Catalogo.Context;
using API_Catalogo.DTOs.Mappings;
using API_Catalogo.Filters;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
    AddJsonOptions(options => 
    options.
    JsonSerializerOptions.
    ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScoped<ApiLoggingFilter, ApiLoggingFilter>();
builder.Services.AddEndpointsApiExplorer();
//habilitando o swagger para aceitar o token no header
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalogo", Version = "v1" });

    //resolvendo conflitos de vers�es no swagger
    c.ResolveConflictingActions(c => c.First());


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header de autoriza��o JWT usando o esquema Bearer. \r\n\r\nInforme 'Bearer' [espa�o] e o seu token.\r\n\r\nExemplo: \'Bearer 12345abcdef\' ",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

string stringConection = Environment.GetEnvironmentVariable("DATABASE");
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

string connectionString = !string.IsNullOrEmpty(stringConection) ? stringConection : mySqlConnection;

Console.Write(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
            ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
        });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x.AllowAnyOrigin());

app.MapControllers();
app.Run();