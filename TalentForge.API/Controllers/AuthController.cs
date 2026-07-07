using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IPasswordHasher<User> hasher, IConfiguration config)
    {
        _context = context;
        _hasher = hasher;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string email, string password, string role)
    {
        var user = new User { Email = email, Role = role };
        user.PasswordHash = _hasher.HashPassword(user, password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("Registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return Unauthorized();

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) return Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}