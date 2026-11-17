
using Microsoft.EntityFrameworkCore;
using TimeSheet.Application;
using TimeSheet.Infrastructure;
using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();



var jwtSettings=builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Missing Jwt section in the configuration");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>
    {
       options.TokenValidationParameters=new TokenValidationParameters
       {
         ValidateIssuer=true,
         ValidIssuer=jwtSettings.Issuer,
         ValidateAudience=true,
         ValidAudience=jwtSettings.Audience,
         ValidateIssuerSigningKey=true,
         IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
         ValidateLifetime=true,
         ClockSkew=TimeSpan.Zero

       };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope=app.Services.CreateScope();
    var seeder=scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();