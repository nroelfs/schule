using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ServerAppSchule.Data;
using ServerAppSchule.Factories;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;
using System.Data;
using System.Diagnostics;

namespace ServerAppSchule.Services
{
    public class UserService : IUserService
    {
        #region private fields
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;
        IConfiguration _configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
        private readonly ApplicationDbContextFactory _contextFactory;
        #endregion
        #region constructor
        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ApplicationDbContextFactory contextFactory)
        {

            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
            _userManager = userManager;
        }
        #endregion
        #region private Methods
        /// <summary>
        /// erstellt einen User für das Linux System
        /// </summary>
        /// <param name="user">User Daten die vom Eingabe Formular übergeben wurden</param>
        private void CreateSysUser(RegisterUser user)
        {
            Process process = new Process();
            process.StartInfo.FileName = "sudo";
            process.StartInfo.Arguments = $"useradd -m {user.UserName}";
            process.Start();
            process.WaitForExit();

            process.StartInfo.Arguments = $"chpasswd";
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.WriteLine($"{user.UserName}:{user.Password}");
            process.StandardInput.Close();
            process.WaitForExit();
        }
        /// <summary>
        /// Updated das Passwort eines Users auf dem Linux System
        /// </summary>
        /// <param name="username">Benutzername</param>
        /// <param name="newPassword">neues PAsswort</param>
        private void UpdateSysUserPassword(string username, string newPassword)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "passwd";
                process.StartInfo.Arguments = $"--stdin {username}";
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine(newPassword);
                process.StandardInput.WriteLine(newPassword);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
            }
        }
        /// <summary>
        /// erstellt einen User der sich in der App anmelden kann
        /// </summary>
        /// <param name="registerUser">User Daten die vom Eingabe Formular übergeben wurden</param>
        /// <returns></returns>
        private async Task CreateAppUserAsync(RegisterUser registerUser)
        {
            var user = new User
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                EmailConfirmed = true,

            };
            await _userManager.CreateAsync(user, registerUser.Password);
            await _userManager.AddToRoleAsync(user, registerUser.Role);
        }
        /// <summary>
        /// Updated das Passwort eines Users
        /// </summary>
        /// <param name="uid"> User Id</param>
        /// <param name="newPassword">Neues  Passwort</param>
        /// <returns></returns>
        private async Task UpdateAppUserPassword(string uid, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(uid);
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
        }
        /// <summary>
        /// Erstellt einen User in MySQL
        /// </summary>
        /// <param name="registerUser">User Daten die vom Eingabe Formular übergeben wurden</param>
        private void CreateMySQLUser(RegisterUser registerUser)
        {
            string database = "projektgruppea";
            string connectionString = _configuration.GetConnectionString("MySQLBase");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandText = "CREATE USER @username IDENTIFIED BY @password";
                        cmd.Parameters.AddWithValue("@username", registerUser.UserName);
                        cmd.Parameters.AddWithValue("@password", registerUser.Password);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"GRANT ALL PRIVILEGES ON `{database}`.* TO @username";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@username", registerUser.UserName);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "FLUSH PRIVILEGES";
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        Console.WriteLine($"User '{registerUser.UserName}' created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
            }
        }
        /// <summary>
        /// Updated das Passwort eines Users in MySQL
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        private void UpdateMySQLUserPassword(string username, string newPassword)
        {
            string connectionString = _configuration.GetConnectionString("MySQLBase");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandText = "SET PASSWORD FOR @username = @password";
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", newPassword);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "FLUSH PRIVILEGES";
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        Console.WriteLine($"User '{username}' password updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user password: {ex.Message}");
            }
        }   

        #endregion
        #region public methods

        public async Task<Task> CreateNewUserAsync(RegisterUser user)
        {
            try
            {
                await CreateAppUserAsync(user);
                CreateSysUser(user);
                CreateMySQLUser(user);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public async Task<IEnumerable<UserSlim>> GetAllMappedUsersAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var users = await context.Users.ToListAsync();
            var userRoles = await context.UserRoles.ToListAsync();
            var roles = await context.Roles.ToListAsync();
            IEnumerable<Task<UserSlim>> userSlimList = users.Select(async user =>
            {
                var userSlim = new UserSlim
                {
                    Email = user.Email,
                    Role = roles.First(r => r.Id == userRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId).Name ?? string.Empty,
                    Username = user.UserName,
                    Id = user.Id
                };
                return userSlim;
            });
            return await Task.WhenAll(userSlimList);
        }

        public RegisterUser GetUserById(string id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            User user = context.Users.FirstOrDefault(u => u.Id == id);
            List<IdentityRole>? roles = context.Roles.ToList();
            RegisterUser registerUser = new RegisterUser
            {
                Email = user.Email,
                UserName = user.UserName,
                Role = roles.Find(roles => roles.Id == context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId).Name ?? string.Empty,
                Id = user.Id
            };
            return registerUser;
        }


        public async Task UpdateLastLoginRefresh(string uid)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            User user = context.Users.FirstOrDefault(u => u.Id == uid);
            user.LastHomeRefresh = DateTime.Now.ToString();
            await context.SaveChangesAsync();
        }

        public async Task<Task> ChangePassword(string uid, string pwd)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            string usrname = context.Users.FirstOrDefault(u => u.Id == uid).UserName;
            try
            {
                await UpdateAppUserPassword(uid, pwd);
                UpdateSysUserPassword(usrname, pwd);
                UpdateMySQLUserPassword(usrname, pwd);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }

        }
        public string GetUsernameById(string uid)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            return context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == uid).UserName ?? "";
        }
        public async Task<bool> IsFirstSignInDone(string uid)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == uid);
            if (user != null && !user.HasFirstLoginDone)
            {
                user.HasFirstLoginDone = true;
                await context.SaveChangesAsync();
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

    }
}
