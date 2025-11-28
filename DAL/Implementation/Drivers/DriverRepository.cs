using AppModels.Entities.Drivers;
using DAL.Interface.Drivers;

namespace DAL.Implementation.Drivers
{
    public class DriverRepository(FAContext context)
        : BaseRepo<Driver>(context), IDriverRepository
    {
    }
}
