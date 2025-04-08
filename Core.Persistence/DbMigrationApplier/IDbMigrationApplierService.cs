using Microsoft.EntityFrameworkCore;

namespace Persistence.DbMigrationApplier;

public interface IDbMigrationApplierService
{
    void Initialize();
}

public interface IDbMigrationApplierService<TDbContext> : IDbMigrationApplierService where TDbContext : DbContext
{
    
}