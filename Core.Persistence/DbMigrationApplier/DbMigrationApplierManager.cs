using Microsoft.EntityFrameworkCore;

namespace Persistence.DbMigrationApplier;

public class DbMigrationApplierManager<TDbContext> : IDbMigrationApplierService<TDbContext>, IDbMigrationApplierService
where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public DbMigrationApplierManager(TDbContext context) => this._context = context;

    public void Initialize() => this._context.Database.EnsureDbApplied();
}