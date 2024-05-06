using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Refit;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text;
using Taka.App.Deliverer.Infra.Data.Context;
using Taka.App.Rentals.Application.BackgroundService;
using Taka.App.Rentals.Application.Handlers;
using Taka.App.Rentals.Application.Services;
using Taka.App.Rentals.Application.Services.Apis;
using Taka.App.Rentals.Domain.Exceptions;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Infra.Data.Repositories;
using Taka.App.Rentals.Infra.Providers;
using Taka.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = builder.Configuration;

builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDelivererRepository, DelivererRepository>();
builder.Services.AddScoped<IRentalPlanService, RentalPlanService>();
builder.Services.AddScoped<IRentalPlanRepository, RentalPlanRepository>();
builder.Services.AddTransient<AuthenticationDelegatingHandler>();
builder.Services.AddSingleton<JwtTokenProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.UseSerilog((ctx, config) =>
{
    config.Enrich.WithProperty("Microservice", "Taka.App.Rentals.Api")
          .WriteTo.Console()
          .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearch:Url"] ?? throw new AppException("ElasticSearch configuration not found.")))
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? throw new AppException("JwtSettings not found.") ))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustHaveMicroserviceAccess", policy =>
        policy.RequireAssertion(context =>
        {

            var accessibleMicroservicesJson = context.User.Claims.FirstOrDefault(c => c.Type == "AccessibleMicroservices")?.Value;
            if (accessibleMicroservicesJson != null)
            {
                var accessibleMicroservices = JsonConvert.DeserializeObject<List<int>>(accessibleMicroservicesJson);
                var validated = accessibleMicroservices.Contains(0) || accessibleMicroservices.Contains(3);

                return validated;
            }

            return false;

        }));
});

builder.Services.AddInMemoryRateLimiting();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(CheckRentalAvailabilityHandler).Assembly);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice Rental Motorcycle", Version = "v1" });

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
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Taka.App.Rentals.Domain"));
builder.Services.AddHostedService<HealthCheckBackgroundService>();

var delivererBaseAddress = builder.Configuration["ApiSettings:DelivererBaseAddress"] ?? throw new AppException("Parameter DelivererBaseAddress not found");
builder.Services.AddRefitClient<IDelivererApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(delivererBaseAddress))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>();


var motorcycleBaseAddress = builder.Configuration["ApiSettings:MotorcycleBaseAddress"] ?? throw new AppException("Parameter MotorcycleBaseAddress not found");
builder.Services.AddRefitClient<IMotorcycleApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(motorcycleBaseAddress))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>(); 


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