using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.ORM;

public static class DatabaseInitializer
{
    public static IServiceCollection InitializeDatabase(this IServiceCollection services)
    {
        try
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Database.Migrate();

            return services;
        }
        catch (Exception)
        {
            return services;
        }
    }
}