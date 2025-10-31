using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace DAL.Interface
{
    public interface IBaseRepo<TEntity>
       where TEntity : class, new()
    {
        /// <summary>
        /// Get all data objects
        /// </summary>
        IQueryable<TEntity> All { get; }
        /// <summary>
        /// Get all data objects without tracking
        /// </summary>
        IQueryable<TEntity> AllWithTrack { get; }
        /// <summary>
        /// Get all data objects after including foreign key fields
        /// </summary>
        /// <param name="includeProperties">Fields to include</param>
        /// <returns></returns>
        IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// Get all data objects after including foreign key fields with no tracking
        /// </summary>
        /// <param name="includeProperties">Fields to include</param>
        /// <returns></returns>
        IQueryable<TEntity> AllWithTrackIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// Find data object with it's keys
        /// </summary>
        /// <param name="keys">The primary key values of the data object</param>
        /// <returns>The found data object </returns>
        TEntity Find(params object[] keys);
        /// <summary>
        /// Find data objects by search condition
        /// </summary>
        /// <param name="predicate">A predicate to filter objects with</param>
        /// /// <param name="noTrack">Whether to track or not track changes on found data objects</param>
        /// <returns>Found data objects</returns>
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, bool withTrack = true);
        IQueryable<TEntity> FindByTracking(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Insert new data objects into the data context
        /// </summary>
        /// <param name="entities">New data objects to insert into the data context </param>
        void Insert(params TEntity[] entities);
        /// <summary>
        /// Delete data objects from the data context
        /// </summary>
        /// <param name="keys">The primary keys of the data objects to delete</param>
        void Delete(params object[] keys);
        /// <summary>
        /// Delete data objects from the data context
        /// </summary>
        /// <param name="entities">Data objects to delete from the data context</param>
        void Delete(params TEntity[] entities);
        void Delete(TEntity entity);
        /// <summary>
        /// Update data objects
        /// </summary>
        /// <param name="entities">Data objects to be updated</param>
        void Update(params TEntity[] entities);
        void Update(IEnumerable<TEntity> entities);
        int ExecuteSqlCommand(FormattableString query);
        int ExecuteSqlCommand(string query, params SqlParameter[] parameters);
        bool Any();
        IQueryable<T> SqlQuery<T>(FormattableString query);
        IQueryable<T> SqlQuery<T>(string query, params SqlParameter[] parameters);

        IQueryable<TEntity> FindByIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        #region async actions

        /// <summary>
        /// Find data object with it's keys
        /// </summary>
        /// <param name="keys">The primary key values of the data object</param>
        /// <returns>The found data object </returns>
        Task<TEntity?> FindAsync(params object[] keys);
        /// <summary>
        /// Insert new data objects into the data context
        /// </summary>
        /// <param name="entities">New data objects to insert into the data context </param>
        Task InsertAsync(params TEntity[] entities);

        #endregion

    }
}
