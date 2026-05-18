using AutoMapper;
using GestionITM.API.Mappings;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure;
using GestionITM.API.Middleware;
using GestionITM.Infrastructure.Repositories;
using GestionITM.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Reflection;
using System.IO;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Arrancando el servidor GestionITM API...");

    builder.Host.UseSerilog((context, services, configuration) => configuration
                     .ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext());

    builder.Logging.AddFilter("Microsoft.AspNetCore.DataProtection", LogLevel.Error);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // --- SWAGGER CONFIGURADO CON CANDADO JWT ---
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionITM API", Version = "v1" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

        // Definición de Seguridad para el Candado
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization usando el esquema Bearer. Ejemplo: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] {}
            }
        });
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // CONFIGURACIÓN DE JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });

    // REPOSITORIOS Y SERVICIOS
    builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
    builder.Services.AddScoped<IEstudianteService, EstudianteService>();
    builder.Services.AddScoped<ICursoRepository, CursoRepository>();
    builder.Services.AddScoped<IProfesorRepository, ProfesorRepository>();
    builder.Services.AddScoped<IProfesorService, ProfesorService>();
    builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
    builder.Services.AddScoped<IMatriculaService, MatriculaService>();

    builder.Services.AddAutoMapper(typeof(MappingProfile));

    var app = builder.Build();

    // MIGRACIONES AUTOMÁTICAS
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ocurrió un error al aplicar la migración.");
        }
    }

    app.UseSerilogRequestLogging();
    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fallo catastrófico al iniciar la API.");
}
finally
{
    Log.Information("Apagando la API de forma segura.");
    Log.CloseAndFlush();
}