using DAL.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Implementation
{
    public class BaseRepo<TEntity> : IBaseRepo<TEntity>
         where TEntity : class, new()
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly DbContext _dbContext;
        /// <summary>
        /// Data Set of context model
        /// </summary>
        private readonly DbSet<TEntity> _dbSet;
        /// <summary>
        /// Repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseRepo(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();

        }
        /// <summary>
        /// Get all data objects
        /// </summary>
        public IQueryable<TEntity> All => _dbSet.AsNoTracking();
        /// <summary>
        /// Get all data objects with tracking
        /// </summary>
        public IQueryable<TEntity> AllWithTrack => _dbSet;
        /// <summary>
        /// Get all data objects after including foreign key fields
        /// </summary>
        /// <param name="includeProperties">Fields to include</param>
        /// <returns></returns>
        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(All, (current, includeProperty) => current.Include(includeProperty));
        }
        /// <summary>
        /// Get all data objects after including foreign key fields with no tracking
        /// </summary>
        /// <param name="includeProperties">Fields to include</param>
        /// <returns></returns>
        public IQueryable<TEntity> AllWithTrackIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(AllWithTrack, (current, includeProperty) => current.Include(includeProperty));
        }
        /// <summary>
        /// Find data object with it's keys
        /// </summary>
        /// <param name="keys">The primary key values of the data object</param>
        /// <returns>The found data object </returns>
        public TEntity Find(params object[] keys)
        {
            return _dbSet.Find(keys);
        }

        /// <summary>
        /// Find data objects by search condition
        /// </summary>
        /// <param name="predicate">A predicate to filter objects with</param>
        /// /// <param name="withTrack">Whether to track or not track changes on found data objects</param>
        /// <returns>Found data objects</returns>
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, bool withTrack = false)
        {
            return withTrack ? AllWithTrack.Where(predicate) : All.Where(predicate);
        }
        public bool Any()
        {
            return _dbSet.Any();
        }
        public virtual IQueryable<TEntity> FindByTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return AllWithTrack.Where(predicate);
        }


        /// <summary>
        /// Insert new data objects into the data context
        /// </summary>
        /// <param name="entities">New data objects to insert into the data context </param>
        public virtual void Insert(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
        }
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }
        /// <summary>
        /// Delete data objects from the data context
        /// </summary>
        /// <param name="keys">The primary keys of the data objects to delete</param>
        public virtual void Delete(params object[] keys)
        {
            var foundEntities = keys.Select(key => Find(key));
            _dbSet.RemoveRange(foundEntities);
        }

        /// <summary>
        /// Delete data objects from the data context
        /// </summary>
        /// <param name="entities">Data objects to delete from the data context</param>
        public virtual void Delete(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }


        /// <summary>
        /// Update data objects
        /// </summary>
        /// <param name="entities">Data objects to be updated</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            _dbContext.ChangeTracker.Clear();
            entities.ToList().ForEach(item =>
            {
                if (!_dbSet.Local.Any(e => e == item))
                    _dbSet.Attach(item);
                _dbSet.Update(item);
            });
        }
        public virtual void Update(params TEntity[] entities)
        {
            _dbContext.ChangeTracker.Clear();
            entities.ToList().ForEach(item =>
            {
                if (!_dbSet.Local.Any(e => e == item))
                    _dbSet.Attach(item);
                _dbSet.Update(item);
            });
        }

        public IQueryable<TEntity> FindByIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(All.Where(predicate), (current, includeProperty) => current.Include(includeProperty));
        }

        public int ExecuteSqlCommand(FormattableString query) => _dbContext.Database.ExecuteSql(query);

        public int ExecuteSqlCommand(string query, params SqlParameter[] parameters) => _dbContext.Database.ExecuteSqlRaw(query, parameters);

        public IQueryable<T> SqlQuery<T>(FormattableString query) => _dbContext.Database.SqlQuery<T>(query);

        public IQueryable<T> SqlQuery<T>(string query, params SqlParameter[] parameters) => _dbContext.Database.SqlQueryRaw<T>(query, parameters);


        #region async implementation
        /// <summary>
        /// Find data object with it's keys
        /// </summary>
        /// <param name="keys">The primary key values of the data object</param>
        /// <returns>The found data object </returns>
        public async Task<TEntity?> FindAsync(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }
        /// <summary>
        /// Insert new data objects into the data context
        /// </summary>
        /// <param name="entities">New data objects to insert into the data context </param>
        public virtual async Task InsertAsync(params TEntity[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion
    }
}
