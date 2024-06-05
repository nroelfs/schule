using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ProjektGruppeAWebApi.Models;
using ProjektGruppeWebApi;
using ProjektGruppeAWebApi.Services;
using System.Diagnostics;

namespace ProjektGruppeAWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        #region private fields
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ProjektGruppeAContext _context;
        private UserService _userService;
        #endregion
        #region public constructors
        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ProjektGruppeAContext context, UserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _userService = userService;
        }
        #endregion
        #region public methods
        [HttpPost("register")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Username);

            if (existingUser != null)
            {
                return BadRequest("Benutzername ist bereits vergeben.");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Role = model.RoleName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    if (await _roleManager.RoleExistsAsync(model.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    }
                    else
                    {
                        return BadRequest("Rolle nicht gefunden.");
                    }
                }

                CreateSysUser(user, model.Password);
                return Ok("Benutzer wurde erfolgreich registriert und der Rolle hinzugefügt.");
            }

            return BadRequest(result.Errors);
        }
        [HttpGet("getAll")]
        [Authorize(Policy = "AdminPolicy")]
        public List<UserSlim> GetAll()
        {
            User[] users = _context.Users
                .ToArray();
            List<UserSlim> usersSlimmed = new();
            foreach (User user in users)
            {
                usersSlimmed.Add(_userService.ToSlimModel(user));
            }
            return usersSlimmed;
        }
        [HttpGet("get/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public UserSlim? Get(string id)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Id  == id);
            if (user != null)
            {
                return _userService.ToSlimModel(user);
            }
            else
            {
                return null;
            }
        }
        [HttpPost("update/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserSlim model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.UserName);

            if (existingUser == null)
            {
                return BadRequest("Benutzername ist nicht vorhanden.");
            }

            var user = new User
            {
                Id = id,
                UserName = model.UserName,
                Email = model.Email,
                Role = model.Role,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };  
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded && existingUser.Role != user.Role)
            {
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _userManager.RemoveFromRoleAsync(user, existingUser.Role);
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else
                {
                    return BadRequest("Rolle nicht gefunden.");
                }
            }
            return Ok("Benutzer erfolgreich aktualisiert.");
        }
        [HttpGet("roles")]
        [Authorize(Policy = "AdminPolicy")]
        public List<string> GetRoles()
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }
        #endregion
        #region private Methods
        private void CreateSysUser(User user, string password)
        {
            Process process = new Process();
            process.StartInfo.FileName = "sudo";
            process.StartInfo.Arguments = $"useradd -m {user.UserName}";
            process.Start();
            process.WaitForExit();

            process.StartInfo.Arguments = $"chpasswd";
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.WriteLine($"{user.UserName}:{password}");
            process.StandardInput.Close();
            process.WaitForExit();
        }
        #endregion
    }
}
