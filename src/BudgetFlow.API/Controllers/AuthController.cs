using BudgetFlow.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BudgetFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService) => _jwtService = jwtService;

    [HttpPost("token")]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        if (request.Email == "demo@budgetflow.com" && request.Password == "demo123")
        {
            var token = _jwtService.GenerateToken(Guid.NewGuid(), request.Email);
            return Ok(new { token });
        }
        return Unauthorized();
    }
}

public record LoginRequest(string Email, string Password);
