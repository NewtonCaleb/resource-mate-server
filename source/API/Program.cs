using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using Mapster;
using NSwag.Generation.Processors.Security;
using SocialWorkApi.API.Infrastructure.Exceptions;
using SocialWorkApi.API.Validators;
using SocialWorkApi.Services.Agencies;
using SocialWorkApi.Services.Auth;
using SocialWorkApi.Services.Database;
using SocialWorkApi.Services.PopulationTypes;
using SocialWorkApi.Services.Services;
using SocialWorkApi.Services.ServiceSubTypes;
using SocialWorkApi.Services.ServiceTypes;
using SocialWorkApi.Services.Users;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("SocialWorkDbOptions"));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Secrets"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7258",
        ValidAudiences = ["https://localhost:7258", "http://localhost:5173", "https://localhost:5173", "http://localhost:4200", "http://192.168.1.224:4200 "],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Secrets").GetValue<string>("JwtSecret")!))
    };
});

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPopulationTypesService, PopulationTypesService>();
builder.Services.AddScoped<IServiceTypesService, ServiceTypesService>();
builder.Services.AddScoped<IServiceSubTypesService, ServiceSubTypesService>();
builder.Services.AddScoped<IAgenciesService, AgenciesService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IAuthService, AuthSerivce>();

builder.Services.AddOpenApiDocument(config =>
{
    config.AddSecurity("JWT",
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.Http,
            Name = "Authorization",
            Description = "Copy 'Bearer ' + JWT Token",
            In = OpenApiSecurityApiKeyLocation.Header,
            Scheme = "Bearer"
        }
    );

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:7258", "http://localhost:4200", "http://192.168.1.224:4200");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowCredentials();
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// var config = new TypeAdapterConfig();
// config.Scan(Assembly.GetExecutingAssembly());

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());


// builder.Services.AddMapster();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
