using AppModels.Entities.Drivers;
using DAL.Interface.Drivers;

namespace DAL.Implementation.Drivers
{
    public class TravelRepository(FAContext context)
        : BaseRepo<Travel>(context), ITravelRepository
    {
    }
}
