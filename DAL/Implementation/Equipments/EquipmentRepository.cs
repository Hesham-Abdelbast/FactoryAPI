using AppModels.Entities.Equipments;
using DAL.Interface.Equipments;

namespace DAL.Implementation.Equipments
{
    public class EquipmentRepository(FAContext context)
        : BaseRepo<Equipment>(context), IEquipmentRepository
    {
    }
}
