using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerAppSchule.Data;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;


namespace ServerAppSchule.Services
{
    public class MigrationService
    {
        #region private fields
        private readonly IHost _host;
        private IUserService _userService;
        #endregion
        #region public constructors
        public MigrationService(IHost host, IUserService userService)
        {
            _host = host;
            _userService = userService;
        }
        #endregion
        #region public Methods
        /// <summary>
        /// Migriert die Datenbank auf den Aktuelsten Stand und Fügt die Stammdaten ein falls diese noch nicht in der Datenbank sind.
        /// </summary>
        public void ApplyMigrations()
        {
            var serviceProvider = _host.Services.CreateScope().ServiceProvider;

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationService>>();

                int maxAttempts = 3;
                int attempts = 0;

                while (attempts < maxAttempts)
                {
                    try
                    {
                        dbContext.Database.Migrate();
                        logger.LogInformation("Migrations applied successfully.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        attempts++;
                        logger.LogError($"Migration attempt {attempts} failed: {ex.Message}");
                        if (attempts >= maxAttempts)
                        {
                            logger.LogError("Maximum migration attempts reached. The application will now exit.");
                            Environment.Exit(1);
                        }
                    }
                }
                RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                Task applySeedData = SeedRoles(roleManager);
                Task applySeedUserData = SeedUsers(userManager, dbContext);
            }
        }
        #endregion
        #region private Methods
        /// <summary>
        /// Erstellt die Basis Rollen die in der Web App verfügbar sein sollen.
        /// </summary>
        /// <param name="roleManager">Refernenz auf die vom Indentity Framework bereitgestellte API für das ASPNETCORE Identity Framework</param>
        ///
        private async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                var userRole = new IdentityRole("User");
                await roleManager.CreateAsync(userRole);
            }
        }
        /// <summary>
        /// Erstellt den ersten User um Zugriff auf die APP zu gewähren
        /// </summary>
        /// <param name="usermanager">Refernenz auf die vom Indentity Framework bereitgestellte API für das ASPNETCORE Identity Framework, für die Klasse User</param>
        /// <param name="dbContext">Aktuelle Datenbank Kontext</param>
        /// <returns></returns>
        private async Task SeedUsers(UserManager<User> usermanager, ApplicationDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                //RegisterUser usr = new RegisterUser
                //{
                //    Email = "service@example.com",
                //    UserName = "Service",
                //    Password = "Admin123-",
                //    Role = "Admin"
                //};
                //await _userService.CreateNewUserAsync(usr);

                //var user = new User
                //{

                //    EmailConfirmed = true,
                //};
                var user = new User
                {
                    UserName = "Service",
                    Email = "user@example.com",

                };
                await usermanager.CreateAsync(user, "Admin123-");
                await usermanager.AddToRoleAsync(user, "Admin");
            }
        }
        #endregion
    }
}
