using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Motorcycle_Group_Ride.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Motorcycle_Group_Ride.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // POST api/authentication/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(registerDto.Username);
                if (userExists != null)
                {
                    return Conflict("Username already exists");
                }

                var newUser = new IdentityUser { UserName = registerDto.Username, Email = registerDto.Email };
                var result = await _userManager.CreateAsync(newUser, registerDto.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(500, $"User creation failed: {result.Errors}");
                }

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/authentication/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var authClaims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.UtcNow.AddHours(2),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }

                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/authentication/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Perform logout logic here if needed (optional for JWT)
                return Ok("User logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/authentication/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(refreshTokenDto.Token);
                var username = principal.Identity.Name; // Retrieve username from JWT token

                // Retrieve the user from your database if necessary
                var user = await _userManager.FindByNameAsync(username);

                // If user is not found or token is invalid, return error
                if (user == null || !ValidateToken(refreshTokenDto.Token, user))
                {
                    return BadRequest("Invalid token");
                }

                // Generate new JWT token (refresh token)
                var authClaims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpPost("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(string userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest($"Change password failed: {result.Errors}");
                }

                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // You might want to validate these based on your needs
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false // Do not validate lifetime here, as we're refreshing the token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private bool ValidateToken(string token, IdentityUser user)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // You might want to validate these based on your needs
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false // Do not validate lifetime here, as we're refreshing the token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            // Additional validation if needed (e.g., user's ID, roles, etc.)
            return true;
        }





    }

}

