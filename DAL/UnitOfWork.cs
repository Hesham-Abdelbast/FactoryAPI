using DAL.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FAContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        public IMaterialTypeRepository MaterialType { get; set; }
        public IMerchantRepository Merchant { get; set; }
        public ITransactionRepository Transaction { get; set; }
        public IStoreInventoryRepository StoreInventory { get; set; }
        public IContactRepository Contact { get; set; }
        public IWarehouseRepositery Warehouse { get; set; }
        public UnitOfWork(
            FAContext dbContext,
            ILogger<UnitOfWork> logger,
            IMaterialTypeRepository materialType,
            IMerchantRepository merchant,
            IStoreInventoryRepository storeInventory,
            ITransactionRepository transaction,
            IContactRepository contact,
            IWarehouseRepositery warehouse)
        {
            _dbContext = dbContext;
            _logger = logger;
            Merchant = merchant;
            MaterialType = materialType;
            StoreInventory = storeInventory;
            Transaction = transaction;
            Contact = contact;
            Warehouse = warehouse;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _transaction?.Dispose();
            _disposed = true;
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public int SaveChanges()
        {
            try
            {
                var res = _dbContext.SaveChanges();
                _dbContext.ChangeTracker.Clear();
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in SaveChanges()");
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var res = await _dbContext.SaveChangesAsync();
                _dbContext.ChangeTracker.Clear();
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in SaveChangesAsync()");
                throw;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var res = await _dbContext.SaveChangesAsync(cancellationToken);
                _dbContext.ChangeTracker.Clear();
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in SaveChangesAsync(CancellationToken)");
                throw;
            }
        }
    }
}
