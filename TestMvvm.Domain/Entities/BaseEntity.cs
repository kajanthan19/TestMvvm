using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMvvm.Domain.Entities
{
    /// <summary>
    /// Holds the princinple capabilities of an entity
    /// </summary>
    /// <typeparam name="TKey">Class, struct</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Primary of entity
        /// </summary>
        TKey Id { get; set; }
    }


    /// <summary>
    /// Provides properties to determine the state of the entity with flags
    /// </summary>
    public interface IFlagableEntity
    {
        /// <summary>
        /// If the entity is equal to be physically deleted 
        /// </summary>
        bool IsDeleted { get; set; }
    }


    /// <summary>
    /// Provides properties to log when changes were made on the entity
    /// </summary>
    public interface ITrackableEntity
    {
        DateTime CreatedOn { get; set; }
        DateTime? LastModifiedOn { get; set; }
    }




    /// <summary>
    /// Base entity will the capability to track modified data and time
    /// </summary>
    public abstract class BaseEntity<TKey> : IEntity<TKey>, IFlagableEntity, ITrackableEntity
    {
        /// <summary>
        /// The primary key of the entity
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }

        /// <summary>
        /// When the entity was first created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// When the entity was last modified
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// Virtual marker to represent if the entity was physically deleted
        /// </summary>
        public bool IsDeleted { get; set; }


        #region Constructor(s)

        public BaseEntity()
        {
            Id = default(TKey);
            CreatedOn = DateTime.Now;
            LastModifiedOn = null;
            IsDeleted = false;
        }

        #endregion

    }
}
