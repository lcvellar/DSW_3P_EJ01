using Microsoft.EntityFrameworkCore;
using DSW_3P_EJ01.Data;
using DSW_3P_EJ01.Repositories;
using DSW_3P_EJ01.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ETAPA 1: almacenamiento en archivo JSON.
// ETAPA 2 (en clase): sustituir SOLO esta implementación por MisionSqlRepository.
//builder.Services.AddScoped<IMisionRepository, MisionSqlRepository>();
builder.Services.AddScoped<IMisionRepository, MisionPostgreSqlRepository>();
builder.Services.AddScoped<IMisionService, MisionService>();
//Configuración del SQL SERVER
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("SqlServerConnection")
//     )
// );
//Configuración de PostgreSQL
builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgreSqlConnection")
    )
);
// Configuración de JWT
var jwt = builder.Configuration.GetSection("Jwt");
var key = Convert.FromBase64String(
   jwt["Key"] ?? throw new InvalidOperationException(
       "Falta configurar Jwt:Key"));
builder.Services
   .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters =
           new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidIssuer = jwt["Issuer"],
               ValidateAudience = true,
               ValidAudience = jwt["Audience"],
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey =
                   new SymmetricSecurityKey(key)
           };
   });

//CONFIGURAR POLÍTICA DE CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("QuestLogFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment() ||
    app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "QuestLog API v1");
    });
}

app.UseCors("QuestLogFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

//ORM
//dotnet add package Microsoft.EntityFrameworkCore.SqlServer
//dotnet add package Microsoft.EntityFrameworkCore.Design
//dotnet tool install --global dotnet-ef --version 10.0.*

//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version "10.0.*"

//dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 10.0.*