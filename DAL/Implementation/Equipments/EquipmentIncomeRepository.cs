using AppModels.Entities.Equipments;
using DAL.Interface.Equipments;

namespace DAL.Implementation.Equipments
{
    public class EquipmentIncomeRepository(FAContext context)
        : BaseRepo<EquipmentIncome>(context), IEquipmentIncomeRepository
    {
    }
}
