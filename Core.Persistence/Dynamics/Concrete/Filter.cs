#region Namespace

namespace Persistence.Dynamics.Concrete
{
    #region Class: Filter
    /// <summary>
    /// Represents a filter definition with various filter criteria.
    /// </summary>
    public class Filter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the field to which the filter is applied.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the operator used in the filter.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the value against which the field is compared.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the logical operator for combining this filter with others.
        /// </summary>
        public string Logic { get; set; }

        /// <summary>
        /// Gets or sets an enumerable collection of additional filters.
        /// </summary>
        public IEnumerable<Filter> Filters { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class
        /// with the specified field, operator, value, logic, and sub-filters.
        /// </summary>
        /// <param name="field">The field to be filtered.</param>
        /// <param name="@operator">The operator to be used in filtering.</param>
        /// <param name="value">The value used in the filter comparison.</param>
        /// <param name="logic">The logical operator for combining filters.</param>
        /// <param name="filters">An enumerable collection of nested filters.</param>
        public Filter(string field, string @operator, string value, string logic, IEnumerable<Filter> filters)
            : this()
        {
            Filters = filters;
            Field = field;
            Operator = @operator;
            Value = value;
            Logic = logic;
        }

        #endregion
    }
    #endregion
}
#endregion
