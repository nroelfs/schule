using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using ProjektGruppeAWebApi.Models;

//Migration Command : dotnet ef migrations add init
//update Command : dotnet ef database update
namespace ProjektGruppeWebApi
{
    public class ProjektGruppeAContext : IdentityDbContext<User, IdentityRole, string>
    {
        public IConfiguration configuration { get; set; }
        public ProjektGruppeAContext(DbContextOptions<ProjektGruppeAContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}
