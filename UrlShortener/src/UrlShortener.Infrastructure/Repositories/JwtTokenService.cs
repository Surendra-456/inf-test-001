using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _key;

    public JwtTokenService()
    {
        _key = "SuperSecretJwtKey12345678901234567890";
    }

    public string GenerateToken(Guid userId,string email)
    {
        var tokenHandler =new JwtSecurityTokenHandler();

        var key =Encoding.UTF8.GetBytes(_key);

        var descriptor =new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString()),

                    new Claim(ClaimTypes.Email,email)
                ]),

                Expires = DateTime.UtcNow.AddHours(8),

                SigningCredentials =new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

        var token =tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }
}