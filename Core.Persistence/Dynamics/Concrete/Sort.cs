#region Namespace

namespace Persistence.Dynamics.Concrete
{
    #region Class: Sort
    /// <summary>
    /// Represents a sorting definition, including field name and direction.
    /// </summary>
    public class Sort
    {
        #region Properties

        /// <summary>
        /// Gets or sets the field to be sorted.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the direction of the sort (e.g., "asc" or "desc").
        /// </summary>
        public string Dir { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sort"/> class.
        /// </summary>
        public Sort()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sort"/> class with specified field and sort direction.
        /// </summary>
        /// <param name="field">The name of the field to sort on.</param>
        /// <param name="dir">The sort direction (e.g., "asc" or "desc").</param>
        public Sort(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }

        #endregion
    }
    #endregion
}
#endregion