#region Namespace

namespace Persistence.Repositories.Concrete
{
    #region Class: Entity
    /// <summary>
    /// Represents a base entity with an identifier.
    /// </summary>
    public class Entity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the primary identifier for this entity.
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class
        /// with a specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the entity.</param>
        public Entity(int id)
            : this()
        {
            Id = id;
        }

        #endregion
    }
    #endregion
}
#endregion