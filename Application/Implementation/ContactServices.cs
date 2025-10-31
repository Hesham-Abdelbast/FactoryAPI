using Application.Interface;
using AppModels.Entities;
using AppModels.Models;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation
{
    public class ContactServices : IContactServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ContactDto> GetContactAsync()
        {
            var contactEntity =await _unitOfWork.Contact.All.FirstOrDefaultAsync();
            var contactDto = _mapper.Map<ContactDto>(contactEntity);
            return contactDto;
        }

        public async Task<bool> UpdateContactAsync(ContactDto contactDto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.Contact
                    .All
                    .FirstOrDefaultAsync(c => c.Id == contactDto.Id);

                if (existing == null)
                {
                    // No existing contact → create a new one
                    var newContact = _mapper.Map<Contact>(contactDto);
                    newContact.CreateDate = DateTime.UtcNow;

                    await _unitOfWork.Contact.InsertAsync(newContact);
                }
                else
                {
                    // Update existing contact
                    _mapper.Map(contactDto, existing);
                    existing.UpdateDate = DateTime.UtcNow;

                    _unitOfWork.Contact.Update(existing);
                }

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"❌ Error while saving contact data: {ex.Message}", ex);
            }
        }

    }
}
