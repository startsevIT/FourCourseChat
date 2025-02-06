using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;
using Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Infrastructure.DataBase.Repos;

class UserRepo : IUserRepo
{
    public async Task<string> LoginAsync(LoginUserDTO dto)
    {
        using SqLiteDbContext db = new();

        User? user = await db.Users.FirstOrDefaultAsync(x => x.Login == dto.Login) 
            ?? throw new Exception("not found user");

        if (user.Password != dto.Password)
            throw new Exception("not correct password");

        List<Claim> claims = [ new ("id", user.Id.ToString()) ];

        var jwt = AuthOptions.CreateToken(claims);
        string token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return token;
    }

    public async Task<GetAccountUserDTO> ReadAsync(Guid id)
    {
        using SqLiteDbContext db = new();
        User user = await db.Users
            .Include(x => x.Chats)
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception("not found user");

        List<GetForListChatDTO> chats = [];
        foreach (var c in user.Chats)
            chats.Add(new (c.Id, c.Name));

        GetAccountUserDTO result = new(user.Name, chats);
        return result;
    }

    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        using SqLiteDbContext db = new();
        User? tryUser = await db.Users.FirstOrDefaultAsync(x => x.Login == dto.Login);
        if (tryUser != null)
            throw new Exception("already exist");
        User user = new(dto.Name,dto.Login, dto.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }
}