using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjektGruppeAWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;

namespace ProjektGruppeAWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region private fields
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        #endregion
        #region public constructor
        public AuthController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        #endregion
        #region public methods
        /// <summary>
        /// Wird  aufgrefufen wenn ein User sich versucht in einer WebApp anzumelden
        /// </summary>
        /// <param name="model">Anmelde Daten der User</param>
        /// <returns> refreshToken: Token der Notwendig zum aktualisieren der Accesstokens ist
        /// token: Accesstoken der Notwendig ist um sich gegenüber der WebAPI zu authentifizieren </returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Dictionary<string, string> model)
        {
            var user = await _userManager.FindByNameAsync(model["Username"]);

            if (user != null && await _userManager.CheckPasswordAsync(user, model["Password"]))
            {
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                    },
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: creds
                );

                var refreshToken = Guid.NewGuid().ToString();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken
                });
            }

            return Unauthorized();
        }
        /// <summary>
        /// Soll den Accesstoken anhand des Refresh Tokens Aktualiseren
        /// </summary>
        /// <param name="refreshToken">der Refreshtoken des Users</param>
        /// <returns>Gibt einen neuen Access und Refresh Token zurueck</returns>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken( [FromBody] string refreshToken)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized();
            }

            // Erstellen Sie einen neuen Access Token
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                },
                expires: DateTime.Now.AddMinutes(15), 
                signingCredentials: creds
            );

            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(1440); 

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = newRefreshToken
            });
        }
        #endregion

    }
}

