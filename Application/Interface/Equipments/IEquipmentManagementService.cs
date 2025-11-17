using AppModels.Common;
using AppModels.Models.Equipments;
using Ejd.GRC.AppModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interface.Equipments
{
    public interface IEquipmentManagementService
    {
        #region Equipment CRUD
        Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync(PaginationEntity param);
        Task<Guid> AddEquipmentAsync(EquipmentDto dto);
        Task<bool> UpdateEquipmentAsync(EquipmentDto dto);
        Task<bool> DeleteEquipmentAsync(Guid id);
        Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id);
        Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync();
        #endregion

        #region Equipment Expenses
        Task<Guid> AddEquipmentExpenseAsync(EquipmentExpenseDto dto);
        Task<bool> DeleteEquipmentExpenseAsync(Guid id);
        Task<IEnumerable<EquipmentExpenseDto>> GetEquipmentExpensesAsync(Guid equipmentId, PaginationEntity param);
        Task<bool> UpdateEquipmentExpenseAsync(EquipmentExpenseDto dto);
        #endregion

        #region Equipment Incomes
        Task<Guid> AddEquipmentIncomeAsync(EquipmentIncomeDto dto);
        Task<bool> DeleteEquipmentIncomeAsync(Guid id);
        Task<IEnumerable<EquipmentIncomeDto>> GetEquipmentIncomesAsync(Guid equipmentId, PaginationEntity param);
        Task<bool> UpdateEquipmentIncomeAsync(EquipmentIncomeDto dto);
        #endregion

        Task<EquipmentFinancialSummaryDto> GetEquipmentFinancialSummaryAsync(Guid equipmentId, ExpenseSummaryRequest request);
    }
}
