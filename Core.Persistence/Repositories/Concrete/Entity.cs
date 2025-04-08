#region Namespace

using Persistence.Repositories.Abstract;

namespace Persistence.Repositories.Concrete
{
    #region Class: Entity
    /// <summary>
    /// Represents a base entity with an identifier.
    /// </summary>
    public class Entity<TId> : IEntityTimestamps
    {
        #region Properties

        /// <summary>
        /// Gets or sets the primary identifier for this entity.
        /// </summary>
        public TId Id { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity() => this.Id = default(TId);

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class
        /// with a specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the entity.</param>
        public Entity(TId id) => this.Id = id;

        #endregion
    }
    #endregion
}
#endregion