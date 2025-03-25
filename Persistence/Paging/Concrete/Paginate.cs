using Persistence.Paging.Abstract;

namespace Persistence.Paging.Concrete;

public class Paginate<T> : IPaginate<T>
{
    internal Paginate(IEnumerable<T> source, int index, int size, int from)
    {
        var enumerable = source as T[] ?? source.ToArray();
        if (from > index) throw new ArgumentException($"From {from} cannot be greater than index {index}.");

        Index = index;
        Size = size;
        if (source is IQueryable<T> queryable)
        {
            Count = queryable.Count();
            Pages = (int)Math.Ceiling((Count / (double)Size));
            From = from;
            Items = queryable.Skip((Index - From) * Size).Take(Size).ToList();
        }
        else
        {
            From = from;
            Count = enumerable.Count();
            Pages = (int)Math.Ceiling((Count / (double)Size));
            Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
        }
        
    }

    internal Paginate()
    {
        Items = Array.Empty<T>();
    }
    public int From { get; set; }
    public int Index { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public IList<T> Items { get; set; }
    public bool HasPrevious => Index - From > 0;
    public bool HasNext => Index - From + 1 < Pages;
}

internal class Paginate<TSource, TResult> : IPaginate<TResult>
{
    public Paginate(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index, int size, int from)
    {
        var enumarable = source as TSource[] ?? source.ToArray();
        if (from > index) throw new ArgumentException($"From {from} cannot be greater than index {index}.");

        Index = index;
        Size = size;
        From = from;
        if (source is IQueryable<TSource> queryable)
        {
            Count = queryable.Count();
            Pages = (int)Math.Ceiling((Count / (double)Size));
            
            var items = queryable.Skip((Index - From) * Size).Take(Size).ToArray();
            Items = new List<TResult>(converter(items));
        }
        else
        {
            Count = enumarable.Length;
            Pages = (int)Math.Ceiling((Count / (double)Size));
            var items = enumarable.Skip((Index - From) * Size).Take(Size).ToArray();
            Items = new List<TResult>(converter(items));
            
        }
    }
    public Paginate(IPaginate<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        Index = source.Index;
        Size = source.Size;
        From = source.From;
        Count = source.Count;
        Pages = source.Pages;

        Items = new List<TResult>(converter(source.Items));
    }
    

    
    public int Index { get; }

    public int Size { get; }

    public int Count { get; }

    public int Pages { get; }

    public int From { get; }

    public IList<TResult> Items { get; }

    public bool HasPrevious => Index - From > 0;

    public bool HasNext => Index - From + 1 < Pages;
}

public static class Paginate
{
    public static IPaginate<T> Empty<T>() => new Paginate<T>();

    public static IPaginate<TResult> From<TResult, TSource>(IPaginate<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        return new Paginate<TSource, TResult>(source, converter);
    }
}