using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DbMigrationApplier;

namespace Persistence.DependencyInjection;

public static class ServiceCollectionDbMigrationApplierExtensions
{
    public static IServiceCollection AddDbMigrationApplier<TDbContext>(this IServiceCollection serviceCollection,
        Func<ServiceProvider, TDbContext> contextFactory) where TDbContext : DbContext
    {
        var buildServiceProvider = serviceCollection.BuildServiceProvider();
        serviceCollection.AddTransient<IDbMigrationApplierService, DbMigrationApplierManager<TDbContext>>(
            (Func<IServiceProvider, DbMigrationApplierManager<TDbContext>>)(_ =>
                new DbMigrationApplierManager<TDbContext>(contextFactory(buildServiceProvider))));

        serviceCollection.AddTransient<IDbMigrationApplierService<TDbContext>, DbMigrationApplierManager<TDbContext>>(
            (Func<IServiceProvider, DbMigrationApplierManager<TDbContext>>)(_ =>
                new DbMigrationApplierManager<TDbContext>(contextFactory(buildServiceProvider))));
        return serviceCollection;
    }
}