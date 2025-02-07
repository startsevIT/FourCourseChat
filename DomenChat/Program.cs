using Domain.business_entities.dtos;
using Infrastructure.Auth;
using Infrastructure.DataBase.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/users/register", async (RegisterUserDTO dto) =>
{
    UserRepo repo = new();
    try
    {
        await repo.RegisterAsync(dto);
        return "User compelete register";
    }
    catch (Exception ex) { return ex.Message; }
    
});
app.MapPost("/users/login", async (LoginUserDTO dto) =>
{
    UserRepo repo = new();
    try
    {
        return await repo.LoginAsync(dto);
    }
    catch(Exception ex) { return ex.Message; }
});
app.MapGet("/users/account", [Authorize] async (HttpContext ctx) =>
{
    Guid id = new(ctx.User.FindFirst("id").Value);
    UserRepo repo = new();
    try 
    {
        return Results.Ok(await repo.ReadAsync(id));
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
    
});
app.Run();
