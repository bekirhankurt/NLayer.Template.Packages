#region Namespace

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Dynamics.Concrete;
using Persistence.Paging.Abstract;
using Persistence.Paging.Concrete;
using Persistence.Repositories.Abstract;

namespace Persistence.Repositories.Concrete
{
    #region Class: EntityFrameworkRepositoryBase
    /// <summary>
    /// Provides a base repository class for performing synchronous and asynchronous operations on entities using Entity Framework Core.
    /// Implements both <see cref="IAsyncRepository{T}"/> and <see cref="IRepository{TEntity}"/> interfaces.
    /// </summary>
    /// <typeparam name="TEntity">The entity type, which must inherit from <see cref="Entity"/>.</typeparam>
    /// <typeparam name="TContext">The EF Core <see cref="DbContext"/> type used for data operations.</typeparam>
    public class EntityFrameworkRepositoryBase<TEntity, TContext> 
        : IAsyncRepository<TEntity>, IRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Gets the EF Core <see cref="DbContext"/> used for data operations.
        /// </summary>
        protected TContext Context { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepositoryBase{TEntity, TContext}"/> class
        /// with a specified <see cref="DbContext"/> for data access.
        /// </summary>
        /// <param name="context">The EF Core context.</param>
        public EntityFrameworkRepositoryBase(TContext context)
        {
            Context = context;
        }

        #endregion

        #region IQuery Implementation

        /// <inheritdoc />
        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        #endregion

        #region IAsyncRepository Implementation

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        /// <inheritdoc/>
        public async Task<IPaginate<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
            int index = 0, 
            int size = 10,
            bool enableTracking = true, 
            CancellationToken cancellationToken = default)
        {
            var query = Query();
            if (!enableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);

            return await query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IPaginate<TEntity>> GetListByDynamicAsync(
            Dynamic dynamic,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
            int index = 0, 
            int size = 10,
            bool enableTracking = true, 
            CancellationToken cancellationToken = default)
        {
            var query = Query().AsQueryable().ToDynamic(dynamic);
            if (!enableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);

            return await query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            await Context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
            return entity;
        }

        #endregion

        #region IRepository Implementation (Synchronous)

        /// <inheritdoc/>
        public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public IPaginate<TEntity> GetList(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0, 
            int size = 10,
            bool enableTracking = true)
        {
            IQueryable<TEntity> query = Query();
            if (!enableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).ToPaginate(index, size);

            return query.ToPaginate(index, size);
        }

        /// <inheritdoc/>
        public IPaginate<TEntity> GetListByDynamic(
            Dynamic dynamic,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
            int index = 0, 
            int size = 10, 
            bool enableTracking = true)
        {
            IQueryable<TEntity> query = Query().AsQueryable().ToDynamic(dynamic);
            if (!enableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);

            return query.ToPaginate(index, size);
        }

        /// <inheritdoc/>
        public TEntity Add(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
            return entity;
        }

        /// <inheritdoc/>
        public TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }

        /// <inheritdoc/>
        public TEntity Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
            return entity;
        }

        #endregion
    }
    #endregion
}
#endregion
