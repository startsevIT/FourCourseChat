using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Auth;

public static class AuthOptions
{
    const string SECRET = "mysupersecret_secretsecretsecretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new(Encoding.UTF8.GetBytes(SECRET));

    public static string CreateToken(Dictionary<string, string> data)
    {
        List<Claim> claims = [];
        foreach (var pair in data)
            claims.Add(new Claim(pair.Key, pair.Value));

        var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(100));
        var credentials = new SigningCredentials(GetSymmetricSecurityKey(), "HS256");

        JwtSecurityToken tokenObj = new(
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenObj);
        return token;
    }
}
