using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using stylHUB.Data_layer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "ASP.NET Core Web API with JWT Authentication"
    });

    const string schemeId = "Bearer";

    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT token.\nExample: Bearer eyJhbGciOiJIUzI1NiIs...",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(schemeId, document),
            new List<string>()
        }
    });
});

// =====================================================
// CORS CONFIGURATION
// Allows React (Vite) running on localhost:5173
// to communicate with this ASP.NET Core API
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =====================================================
// DATABASE CONFIGURATION
// Connect ASP.NET Core with SQL Server
// =====================================================
builder.Services.AddDbContext<App_DB_Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================
// JWT AUTHENTICATION CONFIGURATION
// =====================================================

// Read JWT Secret Key from appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"];

// Configure Authentication Service
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

// =====================================================
// AUTHORIZATION
// =====================================================
builder.Services.AddAuthorization();

// =====================================================
// Build Application
// =====================================================
var app = builder.Build();

// =====================================================
// SWAGGER MIDDLEWARE
// =====================================================
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = string.Empty;
});

// =====================================================
// Middleware Pipeline
// =====================================================

app.UseHttpsRedirection();
app.UseCors("ReactPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();