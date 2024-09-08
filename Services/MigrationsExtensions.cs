using Microsoft.EntityFrameworkCore;
using YouCan.Repository;

namespace YouCan.Services;

public static class MigrationsExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using YouCanContext context = scope.ServiceProvider.GetRequiredService<YouCanContext>();
        context.Database.Migrate();
    }
}