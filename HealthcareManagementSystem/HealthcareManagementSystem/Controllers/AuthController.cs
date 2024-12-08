using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
    public AuthController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var userId = await mediator.Send(command);
        return Ok(new { UserId = userId });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
       var token = await mediator.Send(command);
        return Ok(new { Token = token });
    }
}

