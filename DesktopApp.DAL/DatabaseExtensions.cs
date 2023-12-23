using DesktopApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopApp.DAL
{
    public static class DatabaseExtensions
    {
        public async static Task MigrateAndSeedDatabase(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

                await dbContext.Database.MigrateAsync();
                await seeder.SeedAsync();
            }
        }
    }
}