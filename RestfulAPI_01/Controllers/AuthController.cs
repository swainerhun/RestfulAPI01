using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestfulAPI_01.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestfulAPI_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            IdentityUser? user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            IdentityResult? result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Sikeres regisztráció!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Hibás email vagy jelszó.");

            string? token = GenerateToken(user);
            return Ok(new { token });
        }

        private string GenerateToken(IdentityUser user)
        {
            string? secret = _configuration["JwtSettings:Secret"]!;
            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials? credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Claim[]? claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            JwtSecurityToken? token = new JwtSecurityToken(

                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["JwtSettings:ExpiresInMinutes"]!)),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
