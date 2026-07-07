using stylHUB.Data_layer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Add Controller Services
// =====================================================
builder.Services.AddControllers();

// =====================================================
// CORS CONFIGURATION
// Allows React (Vite) running on localhost:5173
// to communicate with this ASP.NET Core API
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React URL
              .AllowAnyHeader()                     // Allow headers like Authorization
              .AllowAnyMethod();                    // Allow GET, POST, PUT, DELETE, etc.
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
    // Default authentication scheme
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // Default challenge scheme
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validate Issuer
        ValidateIssuer = true,

        // Validate Audience
        ValidateAudience = true,

        // Validate Token Expiration
        ValidateLifetime = true,

        // Validate Signing Key
        ValidateIssuerSigningKey = true,

        // Issuer from appsettings.json
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        // Audience from appsettings.json
        ValidAudience = builder.Configuration["Jwt:Audience"],

        // Secret Key
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

// =====================================================
// Build Application
// =====================================================
var app = builder.Build();

// =====================================================
// Middleware Pipeline
// =====================================================

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Enable CORS
// This must come BEFORE Authentication and Authorization
app.UseCors("ReactPolicy");

// Enable JWT Authentication
app.UseAuthentication();

// Enable Authorization
app.UseAuthorization();

// Map Controller Endpoints
app.MapControllers();

// Run Application
app.Run();