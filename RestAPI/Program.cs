using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Middleware;
using RestAPI.Models;
using RestAPI.Repositories;
using RestAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"]!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(key),

            ClockSkew = TimeSpan.Zero
        };
});


builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRepository<Note>, NoteRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<NoteContextPG>(opt =>
    {
        opt.UseNpgsql(builder.Configuration.GetConnectionString("NoteContextPG"));
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();