using BudgetFlow.Domain.Entities;
using BudgetFlow.Infrastructure.Auth;
using BudgetFlow.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly AppDbContext _db;

    private static readonly Guid DemoUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public AuthController(JwtService jwtService, AppDbContext db)
    {
        _jwtService = jwtService;
        _db = db;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        if (request.Email == "demo@budgetflow.com" && request.Password == "demo123")
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == DemoUserId);
            if (user == null)
            {
                _db.Users.Add(new User
                {
                    Id = DemoUserId,
                    Name = "Demo User",
                    Email = request.Email,
                    PasswordHash = "demo",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                });
                await _db.SaveChangesAsync();
            }

            var token = _jwtService.GenerateToken(DemoUserId, request.Email);
            return Ok(new { token, userId = DemoUserId });
        }
        return Unauthorized();
    }
}

public record LoginRequest(string Email, string Password);
