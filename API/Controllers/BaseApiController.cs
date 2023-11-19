using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
// Take first part of the name of the controller and use that as a route (/api/users)
// When request comes in it will be routed to this controller
public class BaseApiController : ControllerBase
{

}
