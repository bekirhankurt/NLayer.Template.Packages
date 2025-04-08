using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Dynamics.Concrete;
using Persistence.Paging.Abstract;
using Persistence.Paging.Concrete;
using Persistence.Repositories.Abstract;

namespace Persistence.Repositories.Concrete
{
 
    public class EntityFrameworkRepositoryBase<TEntity, TEntityId, TContext> : 
        IAsyncRepository<TEntity, TEntityId>, 
        IRepository<TEntity, TEntityId>,
        IQuery<TEntity> 
        where TEntity : Entity<TEntityId>
        where TContext : DbContext
    {
            protected readonly TContext Context;

    public EntityFrameworkRepositoryBase(TContext context) => this.Context = context;

    public IQueryable<TEntity> Query() => (IQueryable<TEntity>) this.Context.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
      entity.CreatedDate = DateTime.UtcNow;
      var entityEntry = await this.Context.AddAsync<TEntity>(entity, new CancellationToken());
      var num = await this.Context.SaveChangesAsync(new CancellationToken());
      return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
      foreach (TEntity entity in (IEnumerable<TEntity>) entities)
        entity.CreatedDate = DateTime.UtcNow;
      await this.Context.AddRangeAsync((IEnumerable<object>) entities, new CancellationToken());
      var num = await this.Context.SaveChangesAsync(new CancellationToken());
      return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
      entity.UpdatedDate = new DateTime?(DateTime.UtcNow);
      this.Context.Update<TEntity>(entity);
      var num = await this.Context.SaveChangesAsync(new CancellationToken());
      return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
      foreach (TEntity entity in (IEnumerable<TEntity>) entities)
        entity.UpdatedDate = new DateTime?(DateTime.UtcNow);
      this.Context.UpdateRange((IEnumerable<object>) entities);
      var num = await this.Context.SaveChangesAsync(new CancellationToken());
      return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
      await this.SetEntityAsDeletedAsync(entity, permanent);
      var num = await this.Context.SaveChangesAsync(new CancellationToken());
      return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(
      ICollection<TEntity> entities,
      bool permanent = false)
    {
      await this.SetEntityAsDeletedAsync((IEnumerable<TEntity>) entities, permanent);
      var num = await this.Context.SaveChangesAsync(new CancellationToken());
      return entities;
    }

    public async Task<IPaginate<TEntity>> GetListAsync(
      Expression<Func<TEntity, bool>>? predicate = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var source = this.Query();
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      if (predicate != null)
        source = source.Where<TEntity>(predicate);
      return orderBy != null ? await orderBy(source).ToPaginateAsync<TEntity>(index, size, cancellationToken: cancellationToken) : await source.ToPaginateAsync<TEntity>(index, size, cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> GetAsync(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      IQueryable<TEntity> source = this.Query();
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      return await source.FirstOrDefaultAsync<TEntity>(predicate, cancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetListByDynamicAsync(
      Dynamic dynamic,
      Expression<Func<TEntity, bool>>? predicate = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var source = this.Query().ToDynamic<TEntity>(dynamic);
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      if (predicate != null)
        source = source.Where<TEntity>(predicate);
      return await source.ToPaginateAsync<TEntity>(index, size, cancellationToken: cancellationToken);
    }

    public async Task<bool> AnyAsync(
      Expression<Func<TEntity, bool>>? predicate = null,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var source = this.Query();
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      if (predicate != null)
        source = source.Where<TEntity>(predicate);
      return await source.AnyAsync<TEntity>(cancellationToken);
    }

    public TEntity Add(TEntity entity)
    {
      entity.CreatedDate = DateTime.UtcNow;
      this.Context.Add<TEntity>(entity);
      this.Context.SaveChanges();
      return entity;
    }

    public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {
      foreach (var entity in (IEnumerable<TEntity>) entities)
        entity.CreatedDate = DateTime.UtcNow;
      this.Context.AddRange((IEnumerable<object>) entities);
      this.Context.SaveChanges();
      return entities;
    }

    public TEntity Update(TEntity entity)
    {
      entity.UpdatedDate = new DateTime?(DateTime.UtcNow);
      this.Context.Update<TEntity>(entity);
      this.Context.SaveChanges();
      return entity;
    }

    public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
      foreach (TEntity entity in (IEnumerable<TEntity>) entities)
        entity.UpdatedDate = new DateTime?(DateTime.UtcNow);
      this.Context.UpdateRange((IEnumerable<object>) entities);
      this.Context.SaveChanges();
      return entities;
    }

    public TEntity Delete(TEntity entity, bool permanent = false)
    {
      this.SetEntityAsDeleted(entity, permanent);
      this.Context.SaveChanges();
      return entity;
    }

    public ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false)
    {
      this.SetEntityAsDeleted((IEnumerable<TEntity>) entities, permanent);
      this.Context.SaveChanges();
      return entities;
    }

    public TEntity? Get(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      bool withDeleted = false,
      bool enableTracking = true)
    {
      IQueryable<TEntity> source = this.Query();
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      return source.FirstOrDefault<TEntity>(predicate);
    }

    public IPaginate<TEntity> GetList(
      Expression<Func<TEntity, bool>>? predicate = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true)
    {
      var source = this.Query();
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      if (predicate != null)
        source = source.Where<TEntity>(predicate);
      return orderBy != null ? orderBy(source).ToPaginate<TEntity>(index, size) : source.ToPaginate<TEntity>(index, size);
    }

    public IPaginate<TEntity> GetListByDynamic(
      Dynamic dynamic,
      Expression<Func<TEntity, bool>>? predicate = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true)
    {
      var source = this.Query().ToDynamic<TEntity>(dynamic);
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      if (predicate != null)
        source = source.Where<TEntity>(predicate);
      return source.ToPaginate<TEntity>(index, size);
    }

    public bool Any(
      Expression<Func<TEntity, bool>>? predicate = null,
      bool withDeleted = false,
      bool enableTracking = true)
    {
      var source = this.Query();
      if (!enableTracking)
        source = source.AsNoTracking<TEntity>();
      if (withDeleted)
        source = source.IgnoreQueryFilters<TEntity>();
      if (predicate != null)
        source = source.Where<TEntity>(predicate);
      return source.Any<TEntity>();
    }

    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
      if (!permanent)
      {
        this.CheckHasEntityHaveOneToOneRelation(entity);
        await this.setEntityAsSoftDeletedAsync((IEntityTimestamps) entity);
      }
      else
        this.Context.Remove<TEntity>(entity);
    }

    protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
      foreach (TEntity entity in entities)
        await this.SetEntityAsDeletedAsync(entity, permanent);
    }

    protected void SetEntityAsDeleted(TEntity entity, bool permanent)
    {
      if (!permanent)
      {
        this.CheckHasEntityHaveOneToOneRelation(entity);
        this.setEntityAsSoftDeleted((IEntityTimestamps) entity);
      }
      else
        this.Context.Remove<TEntity>(entity);
    }

    protected void SetEntityAsDeleted(IEnumerable<TEntity> entities, bool permanent)
    {
      foreach (TEntity entity in entities)
        this.SetEntityAsDeleted(entity, permanent);
    }

    protected IQueryable<object> GetRelationLoaderQuery(
      IQueryable query,
      Type navigationPropertyType)
    {
      MethodInfo methodInfo1 = ((IEnumerable<MethodInfo>) query.Provider.GetType().GetMethods()).First<MethodInfo>((Func<MethodInfo, bool>) (m => (object) m != null && m.Name == "CreateQuery" && m.IsGenericMethod));
      MethodInfo methodInfo2;
      if ((object) methodInfo1 == null)
        methodInfo2 = (MethodInfo) null;
      else
        methodInfo2 = methodInfo1.MakeGenericMethod(navigationPropertyType);
      if ((object) methodInfo2 == null)
        throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
      return ((IQueryable<object>) methodInfo2.Invoke((object) query.Provider, new object[1]
      {
        (object) query.Expression
      })).Where<object>((Expression<Func<object, bool>>) (x => !((IEntityTimestamps) x).DeletedDate.HasValue));
    }

    protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
      IEnumerable<IForeignKey> foreignKeys = this.Context.Entry<TEntity>(entity).Metadata.GetForeignKeys();
      if ((!foreignKeys.Any<IForeignKey>() ? 0 : (foreignKeys.All<IForeignKey>((Func<IForeignKey, bool>) (x =>
      {
        INavigation dependentToPrincipal = x.DependentToPrincipal;
        if ((dependentToPrincipal != null ? (dependentToPrincipal.IsCollection ? 1 : 0) : 0) == 0)
        {
          INavigation principalToDependent = x.PrincipalToDependent;
          if ((principalToDependent != null ? (principalToDependent.IsCollection ? 1 : 0) : 0) == 0)
            return x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType();
        }
        return true;
      })) ? 1 : 0)) != 0)
        throw new InvalidOperationException("Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key.");
    }

    private async Task setEntityAsSoftDeletedAsync(IEntityTimestamps entity)
    {
      if (entity.DeletedDate.HasValue)
        return;
      entity.DeletedDate = new DateTime?(DateTime.UtcNow);
      foreach (INavigation navigation in this.Context.Entry<IEntityTimestamps>(entity).Metadata.GetNavigations().Where<INavigation>((Func<INavigation, bool>) (x =>
      {
        bool flag;
        if (x != null && !x.IsOnDependent)
        {
          IForeignKey foreignKey = x.ForeignKey;
          if (foreignKey != null)
          {
            switch (foreignKey.DeleteBehavior)
            {
              case DeleteBehavior.Cascade:
              case DeleteBehavior.ClientCascade:
                flag = true;
                goto label_5;
            }
          }
        }
        flag = false;
label_5:
        return flag;
      })).ToList<INavigation>())
      {
        if (!navigation.TargetEntityType.IsOwned() && !(navigation.PropertyInfo == (PropertyInfo) null))
        {
          object entity1 = navigation.PropertyInfo.GetValue((object) entity);
          if (navigation.IsCollection)
          {
            if (entity1 == null)
            {
              entity1 = (object) await this.GetRelationLoaderQuery(this.Context.Entry<IEntityTimestamps>(entity).Collection(navigation.PropertyInfo.Name).Query(), navigation.PropertyInfo.GetType()).ToListAsync<object>();
              if (entity1 == null)
                continue;
            }
            foreach (IEntityTimestamps entity2 in (IEnumerable) entity1)
              await this.setEntityAsSoftDeletedAsync(entity2);
          }
          else
          {
            if (entity1 == null)
            {
              entity1 = await this.GetRelationLoaderQuery(this.Context.Entry<IEntityTimestamps>(entity).Reference(navigation.PropertyInfo.Name).Query(), navigation.PropertyInfo.GetType()).FirstOrDefaultAsync<object>();
              if (entity1 == null)
                continue;
            }
            await this.setEntityAsSoftDeletedAsync((IEntityTimestamps) entity1);
          }
        }
      }
      this.Context.Update<IEntityTimestamps>(entity);
    }

    private void setEntityAsSoftDeleted(IEntityTimestamps entity)
    {
      if (entity.DeletedDate.HasValue)
        return;
      entity.DeletedDate = new DateTime?(DateTime.UtcNow);
      foreach (INavigation navigation in this.Context.Entry<IEntityTimestamps>(entity).Metadata.GetNavigations().Where<INavigation>((Func<INavigation, bool>) (x =>
      {
        bool flag;
        if (x != null && !x.IsOnDependent)
        {
          IForeignKey foreignKey = x.ForeignKey;
          if (foreignKey != null)
          {
            switch (foreignKey.DeleteBehavior)
            {
              case DeleteBehavior.Cascade:
              case DeleteBehavior.ClientCascade:
                flag = true;
                goto label_5;
            }
          }
        }
        flag = false;
label_5:
        return flag;
      })).ToList<INavigation>())
      {
        if (!navigation.TargetEntityType.IsOwned() && !(navigation.PropertyInfo == (PropertyInfo) null))
        {
          object entity1 = navigation.PropertyInfo.GetValue((object) entity);
          if (navigation.IsCollection)
          {
            if (entity1 == null)
            {
              entity1 = (object) this.GetRelationLoaderQuery(this.Context.Entry<IEntityTimestamps>(entity).Collection(navigation.PropertyInfo.Name).Query(), navigation.PropertyInfo.GetType()).ToList<object>();
              if (entity1 == null)
                continue;
            }
            foreach (IEntityTimestamps entity2 in (IEnumerable) entity1)
              this.setEntityAsSoftDeleted(entity2);
          }
          else
          {
            if (entity1 == null)
            {
              entity1 = this.GetRelationLoaderQuery(this.Context.Entry<IEntityTimestamps>(entity).Reference(navigation.PropertyInfo.Name).Query(), navigation.PropertyInfo.GetType()).FirstOrDefault<object>();
              if (entity1 == null)
                continue;
            }
            this.setEntityAsSoftDeleted((IEntityTimestamps) entity1);
          }
        }
      }
      this.Context.Update<IEntityTimestamps>(entity);
    }
    }
    
}