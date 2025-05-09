using System;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class UsersController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUsersByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<AppUser>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }


}
