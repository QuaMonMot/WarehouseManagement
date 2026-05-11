using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

using Warehouse.BLL.Interfaces;

using Warehouse.Models.DTOs;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration
            _configuration;

        private readonly IAuthService
            _authService;

        public AuthController(
            IConfiguration configuration,
            IAuthService authService
        )
        {
            _configuration = configuration;

            _authService = authService;
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        [EndpointSummary("Đăng nhập hệ thống")]

        public IActionResult Login(
            [FromBody] LoginDTO dto
        )
        {
            var user = _authService.Login(
                dto.Username,
                dto.Password
            );

            if (user == null)
            {
                return Unauthorized(
                    "Sai tài khoản hoặc mật khẩu"
                );
            }

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Name,
                    user.Username
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                )
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]
                    )
                );

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],

                    audience:
                        _configuration["Jwt:Audience"],

                    claims: claims,

                    expires:
                        DateTime.Now.AddHours(3),

                    signingCredentials: creds
                );

            return Ok(new
            {
                token =
                    new JwtSecurityTokenHandler()
                    .WriteToken(token),

                role = user.Role
            });
        }
    }
}