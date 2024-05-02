using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text;
using Taka.Common.Middlewares;
using Taka.App.Motor.Application.BackgroundService;
using Taka.App.Motor.Application.Services;
using Taka.App.Motor.Domain.Interfaces;
using Taka.App.Motor.Infra.Data.Context;
using Taka.App.Motor.Infra.Data.Repository;
using Taka.App.Motor.Application.Services.Apis;
using Refit;
using Taka.Common.Infrastructure;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Application.Handlers;
using Taka.App.Motor.Infra.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Taka.App.Motor.Infra.Data.Connections;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ResilienceEngine>();
builder.Services.AddSingleton<IRabbitConnectionFactory, RabbitConnectionFactory>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.UseSerilog((ctx, config) =>
{
    config.Enrich.WithProperty("Microservice", "Taka,App.Motor.Api")
          .WriteTo.Console()
          .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearch:Url"] ?? throw new AppException("Elastic Search configuration not found.")))
          {
              AutoRegisterTemplate = true,
              IndexFormat = $"microservice-logs-{DateTime.UtcNow:yyyy-MM}",
              FailureCallback = (logEvent, exception) => Console.WriteLine($"Unable to submit event: {logEvent.MessageTemplate}, Exception: {exception?.Message}"),
              EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                 EmitEventFailureHandling.WriteToFailureSink |
                                 EmitEventFailureHandling.RaiseCallback,
          });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = "Taka.App.Authentication.Api",            
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? throw new AppException("JwtSettings key Secret not found.")))
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IAuthorizationHandler, MicroserviceAccessHandler>();
builder.Services.AddAuthorization(options =>
 {
     options.AddPolicy("MustHaveMicroserviceAccess", policy =>
         policy.AddRequirements(new MicroserviceAccessRequirement()));
 });


builder.Services.AddInMemoryRateLimiting();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(CheckRentalAvailabilityCommandHandler).Assembly);    
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice Motorcycle", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});


builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Taka.App.Motor.Domain"));
builder.Services.AddHostedService<HealthCheckBackgroundService>();
builder.Services.AddHostedService<RabbitMQConsumerService>();

var rentalBaseAddress = builder.Configuration["ApiSettings:RentalsBaseAddress"] ?? throw new AppException("Parameter RentalsBaseAddress not found");

builder.Services.AddRefitClient<IRentalsApiService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(rentalBaseAddress));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CustomAuthorizationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseIpRateLimiting();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();
