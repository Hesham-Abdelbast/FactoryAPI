using AppModels.Entities;
using DAL.Interface;

namespace DAL.Implementation
{
    public class ContactRepository(FAContext context) : BaseRepo<Contact>(context), IContactRepository
    {
    }
}
