using Persistence.Paging.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Paging.Concrete;

public static class QueryablePaginateExtensions
{
    public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size,
        int from = 0, CancellationToken cancellationToken = default)
    {
        if (from > index) throw new ArgumentException($"From: {from} > Index: {index} must be less than index");
        var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source.Skip((index - from) * 2).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size),
        };

        return list;
    }

    public static IPaginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size, int from = 0)
    {
        if (from > index) throw new ArgumentException($"From: {from} >  index {index} must be less than index");
        var count = source.Count();
        var items = source.Skip((index - from) * size).Take(size).ToList();

        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size),
        };
        return list;
    }
}