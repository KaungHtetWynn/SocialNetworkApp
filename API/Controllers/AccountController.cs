using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// Controller placeholder from first part of the name (/api/account)
public class AccountController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    // [HttpPost("register")]
    // public ActionResult<AppUser> Register([FromBody]AppUser user)
    // {
    //     _context.Users.Add(user);
    //     _context.SaveChanges();

    //     return user;
    // }

    // inside json we are passing object
    // query string automatically bind
    [HttpPost("register")]
    //(string username, string password) // POST: api/account/register?username=dave&password=pwd
    // [FromBody] optional Previously we need because of [ApiController]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName))
        {
            return BadRequest("Username is taken!");
        }

        using var hmac = new HMACSHA512();

        // Use byte array of computed hash (PasswordHash)
        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        // Tracking our new entity in memory
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // AppUser default value is null
        // Find method needs Primary Key
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }

        // Use the key that was generated and saved into our database to get the
        // exact same hashing algorithm that was used when we pass in the same password.
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // loop through each element of the byte array and compare it with the corresponding 
        // elements of the other array.
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }
        //return user;
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    // async task go to database check if user exists
    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync<AppUser>(x => x.UserName == username.ToLower());
    }
}
