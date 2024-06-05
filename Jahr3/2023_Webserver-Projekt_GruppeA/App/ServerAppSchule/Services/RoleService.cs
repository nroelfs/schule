using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerAppSchule.Data;
using ServerAppSchule.Interfaces;

namespace ServerAppSchule.Services
{

    public class RoleService : IRoleService
    {
        #region private fields
        private RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        #endregion
        #region public Constructors
        public RoleService(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        #endregion
        #region public Methods
        public List<string> GetRoleNames()
        {
            List<string> roles = new List<string>();
                roles = _context.Roles
                    .Select(r => r.Name)
                    .AsNoTracking()
                    .ToList();
            return roles;
        }
        public bool RoleExists(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName).Result;
        }
        #endregion

    }
}
