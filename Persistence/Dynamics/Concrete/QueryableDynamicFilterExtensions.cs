using System.Linq.Dynamic.Core;

#region Namespace

namespace Persistence.Dynamics.Concrete
{
    #region Class: QueryableDynamicFilterExtensions
    /// <summary>
    /// Defines extension methods for applying dynamic filters and sorting logic to <see cref="IQueryable{T}"/> instances.
    /// </summary>
    public static class QueryableDynamicFilterExtensions
    {
        #region Fields

        /// <summary>
        /// Provides a mapping between filter operator keywords and their corresponding representations or methods.
        /// </summary>
        private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
        {
            { "eq", "=" },
            { "neq", "!=" },
            { "lt", "<" },
            { "lte", "<=" },
            { "gt", ">" },
            { "gte", ">=" },
            { "isnull", "== null" },
            { "isnotnull", "!= null" },
            { "startswith", "StartsWith" },
            { "endswith", "EndsWith" },
            { "contains", "Contains" },
            { "doesnotcontain", "Contains" }
        };

        #endregion

        #region Methods

        /// <summary>
        /// Applies the dynamic filtering and sorting to the specified <paramref name="query"/> based on the provided <paramref name="dynamic"/> object.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="query">The queryable sequence to which the filters and sorts will be applied.</param>
        /// <param name="dynamic">An instance of <see cref="Dynamic"/> containing filter criteria and sort definitions.</param>
        /// <returns>The queryable sequence after filters and sorts have been applied.</returns>
        public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, Dynamic dynamic)
        {
            if (dynamic.Filter is not null) 
            {
                query = Filter(query, dynamic.Filter);
            }

            if (dynamic.Sorts is not null && dynamic.Sorts.Any()) 
            {
                query = Sort(query, dynamic.Sorts);
            }

            return query;
        }

        /// <summary>
        /// Applies filter logic to the specified queryable sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="query">The sequence to filter.</param>
        /// <param name="filter">A <see cref="Filter"/> instance defining filtering rules.</param>
        /// <returns>The queryable sequence after applying the filter expression.</returns>
        private static IQueryable<T> Filter<T>(IQueryable<T> query, Filter filter)
        {
            var filters = GetAllFilters(filter);
            var values = filters.Select(f => f.Value).ToArray();
            var where = Transform(filter, filters);
            query = query.Where(where, values);
            return query;
        }

        /// <summary>
        /// Applies sorting logic to the specified queryable sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="query">The sequence to sort.</param>
        /// <param name="sorts">A collection of <see cref="Sort"/> definitions indicating fields and direction of sort.</param>
        /// <returns>The queryable sequence after sorting has been applied.</returns>
        private static IQueryable<T> Sort<T>(IQueryable<T> query, IEnumerable<Sort> sorts)
        {
            if (sorts.Any())
            {
                var ordering = string.Join(",", sorts.Select(s => $"{s.Field} {s.Dir}"));
                return query.OrderBy(ordering);
            }

            return query;
        }

        /// <summary>
        /// Retrieves all <see cref="Filter"/> instances, including nested filters, starting from the specified filter.
        /// </summary>
        /// <param name="filter">The root filter from which to gather all nested filters.</param>
        /// <returns>A list of <see cref="Filter"/> objects representing all nested filters.</returns>
        public static IList<Filter> GetAllFilters(Filter filter)
        {
            var filters = new List<Filter>();
            GetFilters(filter, filters);
            return filters;
        }

        /// <summary>
        /// Recursively collects nested filters and adds them to the provided list.
        /// </summary>
        /// <param name="filter">The current filter to process.</param>
        /// <param name="filters">A list that accumulates all discovered filters.</param>
        private static void GetFilters(Filter filter, List<Filter> filters)
        {
            filters.Add(filter);
            if (filter.Filters is not null && filter.Filters.Any())
            {
                foreach (var item in filter.Filters)
                {
                    GetFilters(item, filters);
                }
            }
        }

        /// <summary>
        /// Transforms a <see cref="Filter"/> instance into a dynamic LINQ expression string, incorporating nested filters if necessary.
        /// </summary>
        /// <param name="filter">The filter to transform.</param>
        /// <param name="filters">A list of all discovered filters, including <paramref name="filter"/>.</param>
        /// <returns>A string representing the dynamic LINQ expression used by System.Linq.Dynamic.Core to perform filtering.</returns>
        public static string Transform(Filter filter, IList<Filter> filters)
        {
            var index = filters.IndexOf(filter);
            var comparison = Operators[filter.Operator];
            var where = new System.Text.StringBuilder();

            if (!string.IsNullOrEmpty(filter.Value))
            {
                if (filter.Operator == "doesnotcontain")
                {
                    where.Append($"(!np({filter.Field}).{comparison}(@{index}))");
                }
                else if (comparison == "StartsWith" ||
                         comparison == "EndsWith" ||
                         comparison == "Contains")
                {
                    where.Append($"(np({filter.Field}).{comparison}(@{index}))");
                }
                else
                {
                    where.Append($"np({filter.Field}) {comparison} @{index}");
                }
            }
            else if (filter.Operator == "isnull" || filter.Operator == "isnotnull")
            {
                where.Append($"np({filter.Field}) {comparison}");
            }

            if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
            {
                return $"{where} {filter.Logic} ({string.Join($" {filter.Logic} ", filter.Filters.Select(f => Transform(f, filters)).ToArray())})";
            }

            return where.ToString();
        }

        #endregion
    }
    #endregion
}
#endregion
