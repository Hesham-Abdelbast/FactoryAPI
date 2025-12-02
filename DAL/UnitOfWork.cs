using DAL.Interface;
using DAL.Interface.Drivers;
using DAL.Interface.Employees;
using DAL.Interface.Equipments;
using DAL.Interface.MerchantMangement;
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
        public IMerchantExpenseRepository MerchantExpense { get; set; }
        public IMerchantFinanceRepository MerchantFinance { get; set; }

        public ITransactionRepository Transaction { get; set; }
        public IContactRepository Contact { get; set; }

        public IWarehouseRepositery Warehouse { get; set; }
        public IWarehouseInventoryRepo WarehouseInventory { get; set; }
        public IWarehouseExpenseRepository WarehouseExpense { get; set; }

        public IEmployeeRepository Employees { get; set; }
        public IEmployeeCashAdvanceRepository EmployeeCashAdvance { get; set; }
        public IEmployeeMonthlyPayrollRepository EmployeeMonthlyPayroll { get; set; }
        public IEmployeePersonalExpenseRepository EmployeePersonalExpense { get; set; }

        public IEquipmentRepository Equipments { get; set; }
        public IEquipmentExpenseRepository EquipmentExpense { get; set; }
        public IEquipmentIncomeRepository EquipmentIncome { get; set; }

        public IFinancingRepository Financing { get; set; }

        //Driver info
        public IDriverRepository Driver { get; set; }
        public IDriverExpenseRepository DriverExpense { get; set; }
        public ITravelRepository Travel { get; set; }
        public UnitOfWork(
            FAContext dbContext,
            ILogger<UnitOfWork> logger,
            IMaterialTypeRepository materialType,
            IMerchantRepository merchant,
            ITransactionRepository transaction,
            IContactRepository contact,
            IWarehouseRepositery warehouse,
            IWarehouseInventoryRepo warehouseInventory,
            IWarehouseExpenseRepository warehouseExpense,

            IEmployeeRepository employeeRepository,
            IEmployeeCashAdvanceRepository employeeCashAdvanceRepository,
            IEmployeeMonthlyPayrollRepository employeeMonthlyPayrollRepository,
            IEmployeePersonalExpenseRepository employeePersonalExpenseRepository,

            IEquipmentRepository equipmentRepository,
            IEquipmentExpenseRepository equipmentExpenseRepository,
            IEquipmentIncomeRepository equipmentIncomeRepository,
            IFinancingRepository financing,
            IMerchantExpenseRepository merchantExpenseRepository,
            ITravelRepository travel,
            IDriverExpenseRepository driverExpense,
            IDriverRepository driver,
            IMerchantFinanceRepository merchantFinance)
        {
            _dbContext = dbContext;
            _logger = logger;
            Merchant = merchant;
            MaterialType = materialType;
            Transaction = transaction;
            Contact = contact;

            Warehouse = warehouse;
            WarehouseExpense = warehouseExpense;
            WarehouseInventory = warehouseInventory;

            Employees = employeeRepository;
            EmployeeCashAdvance = employeeCashAdvanceRepository;
            EmployeeMonthlyPayroll = employeeMonthlyPayrollRepository;
            EmployeePersonalExpense = employeePersonalExpenseRepository;

            Equipments = equipmentRepository;
            EquipmentExpense = equipmentExpenseRepository;
            EquipmentIncome = equipmentIncomeRepository;
            Financing = financing;
            MerchantExpense = merchantExpenseRepository;

            Driver = driver;
            DriverExpense = driverExpense;
            Travel = travel;
            MerchantFinance = merchantFinance;
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
