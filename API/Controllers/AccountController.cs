using System.Security.Cryptography;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerDto)
    {
        if(IsUserExists(registerDto.UserName).Result)
        {
            return BadRequest("Username is already exists");
        }
        using var hmac = new HMACSHA512();
        {
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok(new UserDto{
                UserName = user.UserName,
                Token = tokenService.CreateToken(user) 
            });
        }
         
    }

    private async Task<bool> IsUserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login(LoginUserDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
        if(user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));
        for(int i = 0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return Ok(new UserDto{
            UserName = user.UserName,
            Token = tokenService.CreateToken(user) 
        });
    }

}