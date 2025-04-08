using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Persistence.DbMigrationApplier;

public static class DatabaseFacadeDbMigrationApplierExtensions
{
    public static DatabaseFacade EnsureDbApplied(this DatabaseFacade databaseFacade)
    {
        if (!databaseFacade.CanConnect())
            return databaseFacade;
        if (databaseFacade.IsInMemory())
            databaseFacade.EnsureCreated();
        if (databaseFacade.IsRelational())
            databaseFacade.Migrate();

        return databaseFacade;
       



    }
}