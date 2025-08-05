using LogiTrack.Data;
using LogiTrack.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using LogiTrack.Services;

// Environment variable for JWT key because putting it in appsettings.json didn't work somehow.
var key = "b6785d24551f467312cc552a99df67177e3c7e7d";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();


builder.Services.AddDbContext<LogiTrackContext>();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LogiTrackContext>()
    .AddDefaultTokenProviders();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddSingleton<TokenService>();


// Add Authorization.
builder.Services.AddAuthorization(options =>
{
    // Basic Policy for user role
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Manager", "Admin"));

    // Policy for who can edit entries and see product specifics.
    options.AddPolicy("EditorPolicy", policy => policy.RequireRole("Manager", "Admin"));

    // Admin Only Policy
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));

});



// Add Authentication.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });


builder.Services.AddControllers();



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

app.UseHttpsRedirection();


app.MapControllers();

app.Run();