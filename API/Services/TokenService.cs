using System;
using System.Text;
using API.Entities;
using API.Interfaces;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var configKey = config["TokenKey"]?? throw new Exception("TokenKey is not set in the configuration");
        if(configKey.Length < 64)
        {
            throw new Exception("TokenKey must be at least 64 characters long");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}

