using AppModels.Models;

namespace Application.Interface
{
    public interface IContactServices
    {
        Task<ContactDto> GetContactAsync();
        Task<bool> UpdateContactAsync(ContactDto contactDto);
    }
}
