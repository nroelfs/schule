using ServerAppSchule.Services;

namespace ServerAppSchule.Services.BackgroundServices
{
    public class MigrationBackgroundService : IHostedService
    {
        #region private fields
        private readonly IServiceProvider _serviceProvider;
        #endregion
        #region public constructor
        public MigrationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion
        #region public methods
        /// <summary>
        /// Startet den MigrationService
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
                migrationService.ApplyMigrations();
            }

            return Task.CompletedTask;
        }
        /// <summary>
        /// stoppt den MigrationService
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
