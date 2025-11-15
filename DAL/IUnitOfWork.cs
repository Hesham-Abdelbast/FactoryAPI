using DAL.Interface;
using DAL.Interface.Employees;
using DAL.Interface.Equipments;
using DAL.Interface.MerchantMangement;

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
        public IMerchantRepository Merchant { get; set; }
        public IMerchantExpenseRepository MerchantExpense { get; set; }

        public ITransactionRepository Transaction { get; set; }
        public IMaterialTypeRepository MaterialType { get; set; }

        public IWarehouseRepositery Warehouse { get; set; }
        public IWarehouseInventoryRepo WarehouseInventory { get; set; }
        public IWarehouseExpenseRepository WarehouseExpense { get; set; }

        public IContactRepository Contact { get; set; }

        public IEmployeeRepository Employees { get; set; }
        public IEmployeeCashAdvanceRepository EmployeeCashAdvance { get; set; }
        public IEmployeeMonthlyPayrollRepository EmployeeMonthlyPayroll { get; set; }
        public IEmployeePersonalExpenseRepository EmployeePersonalExpense { get; set; }

        public IEquipmentRepository Equipments { get; set; }
        public IEquipmentExpenseRepository EquipmentExpense { get; set; }
        public IEquipmentIncomeRepository EquipmentIncome { get; set; }

        public IFinancingRepository Financing { get; set; }
        
        #endregion
    }
}
