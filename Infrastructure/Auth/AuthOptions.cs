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

    public static JwtSecurityToken CreateToken(List<Claim> claims) => new(
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
        signingCredentials: new SigningCredentials(
            GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256));
}
