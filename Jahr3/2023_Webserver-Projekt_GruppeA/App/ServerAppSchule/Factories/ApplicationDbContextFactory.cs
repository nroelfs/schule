using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerAppSchule.Models;
using ServerAppSchule.Data;

namespace ServerAppSchule.Factories
{
    /// <summary>
    /// Factory zum Erstellen der Datenbank
    /// </summary>
    public class ApplicationDbContextFactory : IDisposable
    {
        #region private members
        private readonly DbContextOptions<ApplicationDbContext> _options;
        #endregion
        #region public constructors
        public ApplicationDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }
        #endregion
        #region public methods
        /// <summary>
        /// Erstellt eine neue Instanz der Datenbank
        /// </summary>
        /// <returns></returns>
        public ApplicationDbContext CreateDbContext()
        {
            return new ApplicationDbContext(_options);
        }
        /// <summary>
        /// Schließt die Instanz der  Datenbank
        /// </summary>
        public void Dispose()
        {
            using (var context = CreateDbContext())
            {
                context.Dispose();
            }
        }
        #endregion
    }
}
