using Application.Interface;
using AppModels.Entities;
using AppModels.Models;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;


namespace Application.Implementation
{
    public class MerchantServices : IMerchantServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MerchantServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(MerchantDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "MerchantDto cannot be null");

                var Merchant = _mapper.Map<Merchant>(entity);
                await _unitOfWork.Merchant.InsertAsync(Merchant);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return Merchant.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"Error while adding Merchant: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(MerchantDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "MerchantDto cannot be null");

                var existing = await _unitOfWork.Merchant.FindAsync(entity.Id);
                if (existing == null)
                    throw new KeyNotFoundException($"Merchant with ID {entity.Id} not found.");

                _mapper.Map(entity, existing);
                _unitOfWork.Merchant.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"Error while updating Merchant: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.Merchant.FindAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException($"Merchant with ID {id} not found.");

                existing.IsDeleted = true;
                _unitOfWork.Merchant.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"Error while deleting Merchant: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<MerchantDto>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.Merchant.All.ToListAsync();
                return _mapper.Map<IEnumerable<MerchantDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching Merchant list: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.Merchant.FindAsync(id) != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while checking Merchant existence: {ex.Message}", ex);
            }
        }

        public async Task<MerchantDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Merchant.FindAsync(id);
                return _mapper.Map<MerchantDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching Merchant by ID: {ex.Message}", ex);
            }
        }
    }
}
