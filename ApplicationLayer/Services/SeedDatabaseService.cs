using DataLayer.Helper;

namespace ApplicationLayer.Services
{
    public static class SeedDatabaseService
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            DbInitializer.Seed(serviceProvider);
        }
    }
}
