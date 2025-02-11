using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;
using Domain.Mapping;
using Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Infrastructure.DataBase.Repos;

public class UserRepo : IUserRepo
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

        //Это
        //List<GetForListChatDTO> chats = [];
        //foreach (var c in user.Chats)
        //    chats.Add(c.Map());


        //И это "[..user.Chats.Select(x => x.Map())]" - одно и то же
        return user.Map([..user.Chats.Select(x => x.Map())]);
    }

    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        using SqLiteDbContext db = new();

        User? tryUser = await db.Users.FirstOrDefaultAsync(x => x.Login == dto.Login);

        if (tryUser != null)
            throw new Exception("already exist");

        db.Users.Add(dto.Map());
        await db.SaveChangesAsync();
    }
}