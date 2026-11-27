using System.Text.Json.Serialization;
using TimeSheet.Application;
using TimeSheet.Application.Auth;
using TimeSheet.Auth.Api.Exceptions;
using TimeSheet.Infrastructure;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers()
        .AddJsonOptions(o=>o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
