using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
// Take first part of the name of the controller and use that as a route (/api/users)
// When request comes in it will be routed to this controller
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    // AppDbContext will be injected when UsersController instance is created
    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {

        // We are going to wait for this method and it will notify us when it is completed
        var users = await _context.Users.ToListAsync();

        // 200 ok response also return by framework
        // letting the framework create correct type of response
        return users;
        //return Ok(users);
        // Explicit return 200 Ok response
    }

    //[HttpGet]
    //[Route("{id}")]
    // api/users/2
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        //var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();

        return await _context.Users.FindAsync(id);
    }
}
