using DAL.Interface;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        void Commit();
        void Rollback();

        #region Repositories
        public ITransactionRepository Transaction { get; set; }
        public IStoreInventoryRepository StoreInventory { get; set; }
        public IMerchantRepository Merchant { get; set; }
        public IMaterialTypeRepository MaterialType { get; set; }
        public IWarehouseRepositery Warehouse { get; set; }
        public IContactRepository Contact { get; set; }

        #endregion
    }
}
