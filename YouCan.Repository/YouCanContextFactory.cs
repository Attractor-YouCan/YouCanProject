using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace YouCan.Repository
{
    public class YouCanContextFactory : IDesignTimeDbContextFactory<YouCanContext>
    {
        public YouCanContext CreateDbContext(string[] args)
        {
            // Ensure that the path to the appsettings.json file is correct.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<YouCanContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new YouCanContext(optionsBuilder.Options);
        }
    }
}