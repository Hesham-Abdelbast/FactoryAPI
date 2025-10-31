using AppModels.Entities;
using DAL.Interface;
namespace DAL.Implementation
{
    public class MaterialTypeRepository(FAContext context) : BaseRepo<MaterialType>(context), IMaterialTypeRepository
    {

    }
}
