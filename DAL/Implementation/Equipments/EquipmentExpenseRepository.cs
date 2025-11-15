using AppModels.Entities.Equipments;
using DAL.Interface.Equipments;

namespace DAL.Implementation.Equipments
{
    public class EquipmentExpenseRepository(FAContext context)
        : BaseRepo<EquipmentExpense>(context), IEquipmentExpenseRepository
    {
    }
}
