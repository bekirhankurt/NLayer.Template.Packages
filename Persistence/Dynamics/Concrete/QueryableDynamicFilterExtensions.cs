using System.Linq.Dynamic.Core;
using System.Text;

namespace Persistence.Dynamics.Concrete;

public static class QueryableDynamicFilterExtensions
{
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


    public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, Dynamic dynamic)
    {
        if(dynamic.Filter is not null) query = Filter(query,dynamic.Filter);
        if (dynamic.Sorts is not null && dynamic.Sorts.Any()) query = Sort(query, dynamic.Sorts);
        return query;
    }

    private static IQueryable<T> Filter<T>(IQueryable<T> query, Filter filter)
    {
        var filters = GetAllFilters(filter);
        var values = filters.Select(f => f.Value).ToArray();
        var where = Transform(filter, filters);
        query = query.Where(where, values);
        return query;
    }

    private static IQueryable<T> Sort<T>(IQueryable<T> query, IEnumerable<Sort> sorts)
    {
        if (sorts.Any())
        {
            var ordering = string.Join(",", sorts.Select(s => $"{s.Field} {s.Dir}"));
            return query.OrderBy(ordering);
        }

        return query;
    }

    public static IList<Filter> GetAllFilters(Filter filter)
    {
        var filters = new List<Filter>();
        GetFilters(filter, filters);
        return filters;
    }

    private static void GetFilters(Filter filter, List<Filter> filters)
    {
        filters.Add(filter);
        if(filter.Filters is not null && filter.Filters.Any())
            foreach (var item in filter.Filters)
            {
                GetFilters(item, filters);
            }
    }

    public static string Transform(Filter filter, IList<Filter> filters)
    {
        var index = filters.IndexOf(filter);
        var comparison = Operators[filter.Operator];
        var where = new StringBuilder();

        if (!string.IsNullOrEmpty(filter.Value))
        {
            if (filter.Operator == "doesnotcontain")
                where.Append($"(!np({filter.Field}).{comparison}(@{index}))");
            else if (comparison == "StartsWith" ||
                     comparison == "EndsWith" ||
                     comparison == "Contains")
                where.Append($"(np({filter.Field}).{comparison}(@{index}))");
            else
                where.Append($"np({filter.Field}) {comparison} @{index}");
        }
        else if (filter.Operator == "isnull" || filter.Operator == "isnotnull")
        {
            where.Append($"np({filter.Field}) {comparison}");
        }

        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
            return
                $"{where} {filter.Logic} ({string.Join($" {filter.Logic} ", filter.Filters.Select(f => Transform(f, filters)).ToArray())})";

        return where.ToString();
    }
    
}