using Microsoft.AspNetCore.Mvc;
using TrainingAppAPI.Models;
using TrainingAppAPI.Services;

namespace TrainingAppAPI.Controllers;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;

    public AuthController(ISessionService sessionService, IUserService userService){
        _userService = userService;
        _sessionService = sessionService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] User user){
        var existsUser = await _userService.FindUserByEmail(user.Email);
        if(existsUser != null){
            return BadRequest("Email already in use");
        }
        User newUser = await _userService.CreateUser(user);
        if(newUser == null){
            // Failed to create the session
            return StatusCode(500, "Failed to create user");
        }
        var session =  await _sessionService.CreateSession(newUser.Id!);
        if (session == null)
        {
            // Failed to create the session
            return StatusCode(500, "Failed to create session");
        }
        
        return Created("User created", new {sessionId = session.Id});
    }
}