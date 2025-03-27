#region Namespace

using Persistence.Paging.Abstract;

namespace Persistence.Paging.Concrete
{
    #region Class: Paginate<T>
    /// <summary>
    /// Provides functionality to paginate a collection or queryable source of type <typeparamref name="T"/>.
    /// Implements the <see cref="IPaginate{T}"/> interface, offering essential pagination properties.
    /// </summary>
    /// <typeparam name="T">The type of elements being paginated.</typeparam>
    public class Paginate<T> : IPaginate<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Paginate{T}"/> class based on a given data source.
        /// </summary>
        /// <param name="source">The collection or <see cref="IQueryable{T}"/> to paginate.</param>
        /// <param name="index">The current page index.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="from">The starting offset used in page calculations.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="from"/> is greater than <paramref name="index"/>.</exception>
        internal Paginate(IEnumerable<T> source, int index, int size, int from)
        {
            var enumerable = source as T[] ?? source.ToArray();
            if (from > index) 
                throw new ArgumentException($"From {from} cannot be greater than index {index}.");

            Index = index;
            Size = size;

            if (source is IQueryable<T> queryable)
            {
                Count = queryable.Count();
                Pages = (int)Math.Ceiling(Count / (double)Size);
                From = from;
                Items = queryable.Skip((Index - From) * Size).Take(Size).ToList();
            }
            else
            {
                From = from;
                Count = enumerable.Length;
                Pages = (int)Math.Ceiling(Count / (double)Size);
                Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paginate{T}"/> class,
        /// creating an empty paginated result with no data.
        /// </summary>
        internal Paginate()
        {
            Items = Array.Empty<T>();
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public int From { get; set; }

        /// <inheritdoc/>
        public int Index { get; set; }

        /// <inheritdoc/>
        public int Size { get; set; }

        /// <inheritdoc/>
        public int Count { get; set; }

        /// <inheritdoc/>
        public int Pages { get; set; }

        /// <inheritdoc/>
        public IList<T> Items { get; set; }

        /// <inheritdoc/>
        public bool HasPrevious => Index - From > 0;

        /// <inheritdoc/>
        public bool HasNext => Index - From + 1 < Pages;

        #endregion
    }
    #endregion

    #region Class: Paginate<TSource, TResult>
    /// <summary>
    /// Provides functionality to paginate a collection or queryable source of type <typeparamref name="TSource"/>,
    /// then converts the resulting items into <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TSource">The original source type of elements.</typeparam>
    /// <typeparam name="TResult">The target type into which items are converted.</typeparam>
    internal class Paginate<TSource, TResult> : IPaginate<TResult>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Paginate{TSource, TResult}"/> class
        /// with a given source, a conversion function, and pagination parameters.
        /// </summary>
        /// <param name="source">The collection or <see cref="IQueryable{TSource}"/> to paginate.</param>
        /// <param name="converter">A function that converts <typeparamref name="TSource"/> items into <typeparamref name="TResult"/> items.</param>
        /// <param name="index">The current page index.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="from">The starting offset used in page calculations.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="from"/> is greater than <paramref name="index"/>.</exception>
        public Paginate(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int index, int size, int from)
        {
            var enumarable = source as TSource[] ?? source.ToArray();
            if (from > index)
                throw new ArgumentException($"From {from} cannot be greater than index {index}.");

            Index = index;
            Size = size;
            From = from;

            if (source is IQueryable<TSource> queryable)
            {
                Count = queryable.Count();
                Pages = (int)Math.Ceiling(Count / (double)Size);

                var items = queryable.Skip((Index - From) * Size).Take(Size).ToArray();
                Items = new List<TResult>(converter(items));
            }
            else
            {
                Count = enumarable.Length;
                Pages = (int)Math.Ceiling(Count / (double)Size);

                var items = enumarable.Skip((Index - From) * Size).Take(Size).ToArray();
                Items = new List<TResult>(converter(items));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paginate{TSource, TResult}"/> class
        /// by reusing pagination data from an existing <see cref="IPaginate{TSource}"/> and converting items.
        /// </summary>
        /// <param name="source">An existing paginated source.</param>
        /// <param name="converter">A function that converts <typeparamref name="TSource"/> items into <typeparamref name="TResult"/> items.</param>
        public Paginate(IPaginate<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            Index = source.Index;
            Size = source.Size;
            From = source.From;
            Count = source.Count;
            Pages = source.Pages;

            Items = new List<TResult>(converter(source.Items));
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public int Index { get; }

        /// <inheritdoc/>
        public int Size { get; }

        /// <inheritdoc/>
        public int Count { get; }

        /// <inheritdoc/>
        public int Pages { get; }

        /// <inheritdoc/>
        public int From { get; }

        /// <inheritdoc/>
        public IList<TResult> Items { get; }

        /// <inheritdoc/>
        public bool HasPrevious => Index - From > 0;

        /// <inheritdoc/>
        public bool HasNext => Index - From + 1 < Pages;

        #endregion
    }
    #endregion

    #region Class: Paginate (Static)
    /// <summary>
    /// Contains factory methods for creating and manipulating paginated results.
    /// </summary>
    public static class Paginate
    {
        #region Methods

        /// <summary>
        /// Creates an empty pagination object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of items to be contained in the paginated object.</typeparam>
        /// <returns>An <see cref="IPaginate{T}"/> with no items.</returns>
        public static IPaginate<T> Empty<T>() => new Paginate<T>();

        /// <summary>
        /// Creates a <see cref="IPaginate{TResult}"/> from an existing paginated source
        /// by converting the items of type <typeparamref name="TSource"/> into <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of items in the resulting paginated object.</typeparam>
        /// <typeparam name="TSource">The original source item type.</typeparam>
        /// <param name="source">An existing pagination object containing items of type <typeparamref name="TSource"/>.</param>
        /// <param name="converter">A function that converts a collection of <typeparamref name="TSource"/> items into <typeparamref name="TResult"/>.</param>
        /// <returns>A new pagination object of type <typeparamref name="TResult"/>.</returns>
        public static IPaginate<TResult> From<TResult, TSource>(
            IPaginate<TSource> source,
            Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            return new Paginate<TSource, TResult>(source, converter);
        }

        #endregion
    }
    #endregion
}
#endregion
