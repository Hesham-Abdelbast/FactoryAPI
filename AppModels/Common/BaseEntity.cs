using System;

namespace AppModels.Common
{
    /// <summary>
    /// Base entity class that includes common auditing properties.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Identifier of the user who created the entity.
        /// </summary>
        public Guid CreateBy { get; set; }

        /// <summary>
        /// Timestamp when the entity was created.
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Identifier of the user who last updated the entity.
        /// </summary>
        public Guid? UpdateBy { get; set; }

        /// <summary>
        /// Timestamp when the entity was last updated.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Indicates whether the entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
