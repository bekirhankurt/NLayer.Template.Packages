namespace Persistence.Repositories.Abstract;

public interface IQuery<out T>
{
    IQueryable<T> Query();
}