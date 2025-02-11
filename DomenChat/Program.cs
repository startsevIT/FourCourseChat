using Domain.business_entities.dtos;
using Infrastructure.Auth;
using Infrastructure.DataBase.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Text;

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

app.UseWebSockets();

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
    catch (Exception ex) { return ex.Message; }
});
app.MapGet("/users/account", [Authorize] async (HttpContext ctx) =>
{
    Guid id = GetIdFromHttpContext(ctx);
    UserRepo repo = new();
    try
    {
        return Results.Ok(await repo.ReadAsync(id));
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }

});

app.MapPost("/chats/create", [Authorize] async (CreateChatDTO dto, HttpContext ctx) =>
{
    Guid userId = GetIdFromHttpContext(ctx);
    ChatRepo repo = new();
    try
    {
        await repo.CreateAsync(dto, userId);
        return "Chat created";
    }
    catch (Exception ex) { return ex.Message; }
});
app.MapGet("/chats/link/{chatId}", [Authorize] async (HttpContext ctx, Guid chatId) =>
{
    Guid userId = GetIdFromHttpContext(ctx);
    ChatRepo repo = new();
    try
    {
        var result = await repo.ReadAndLinkAsync(chatId, userId);
        return Results.Ok(result);
    }
    catch (Exception ex) { return Results.NotFound(ex.Message); }
});


Dictionary<Guid,List<WebSocket>> rooms = [];

app.Map("/ws/{id}", async (HttpContext ctx, Guid id) =>
{
    var websocket = await ctx.WebSockets.AcceptWebSocketAsync();
    byte[] buffer = new byte[1024 * 4];

    if (rooms.ContainsKey(id))
        rooms[id].Add(websocket);
    else
        rooms.Add(id, [websocket]);

    var result = await websocket.ReceiveAsync(new(buffer), CancellationToken.None);

    while (!result.CloseStatus.HasValue)
    {
        string recieveMessage = Encoding.UTF8.GetString(buffer[..result.Count]);


        byte[] sendMessage = Encoding.UTF8.GetBytes(recieveMessage);

        foreach (var connection in rooms[id])
            await connection.SendAsync(sendMessage, result.MessageType, result.EndOfMessage, CancellationToken.None);

        result = await websocket.ReceiveAsync(new(buffer), CancellationToken.None);
    }

    await websocket.CloseAsync(result.CloseStatus.Value,result.CloseStatusDescription,CancellationToken.None);
    rooms[id].Remove(websocket);
});




app.Run();

static Guid GetIdFromHttpContext(HttpContext ctx)
{
    return new(ctx.User.FindFirst("id").Value);
}