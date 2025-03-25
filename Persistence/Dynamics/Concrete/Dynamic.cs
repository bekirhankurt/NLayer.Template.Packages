#region Namespace

namespace Persistence.Dynamics.Concrete
{
    #region Class: Dynamic
    /// <summary>
    /// Represents a dynamic object containing sort definitions and a filter for data operations.
    /// </summary>
    public class Dynamic
    {
        #region Properties

        /// <summary>
        /// Gets or sets a collection of sort criteria.
        /// </summary>
        public IEnumerable<Sort> Sorts { get; set; }

        /// <summary>
        /// Gets or sets the filter criteria.
        /// </summary>
        public Filter Filter { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Dynamic"/> class.
        /// </summary>
        public Dynamic()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dynamic"/> class with the specified sort criteria and filter.
        /// </summary>
        /// <param name="sorts">An enumerable collection of <see cref="Sort"/> objects.</param>
        /// <param name="filter">A <see cref="Filter"/> defining filtering criteria.</param>
        public Dynamic(IEnumerable<Sort> sorts, Filter filter)
        {
        }

        #endregion
    }
    #endregion
}
#endregion