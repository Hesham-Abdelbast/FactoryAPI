using Application.Interface;
using AppModels.Entities;
using AppModels.Models;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation
{
    public class MaterialTypeServices : IMaterialTypeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialTypeServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(MaterialTypeDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "MaterialTypeDto cannot be null");

                var materialType = _mapper.Map<MaterialType>(entity);
                await _unitOfWork.MaterialType.InsertAsync(materialType);
                await _unitOfWork.SaveChangesAsync(); 
                _unitOfWork.Commit();
                return materialType.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"Error while adding MaterialType: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(MaterialTypeDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "MaterialTypeDto cannot be null");

                var existing = await _unitOfWork.MaterialType.FindAsync(entity.Id);
                if (existing == null)
                    throw new KeyNotFoundException($"MaterialType with ID {entity.Id} not found.");

                _mapper.Map(entity, existing);
                _unitOfWork.MaterialType.Update(existing);
                await _unitOfWork.SaveChangesAsync(); 
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"Error while updating MaterialType: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.MaterialType.FindAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException($"MaterialType with ID {id} not found.");

                existing.IsDeleted = true;
                _unitOfWork.MaterialType.Update(existing);
                await _unitOfWork.SaveChangesAsync(); 
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception($"Error while deleting MaterialType: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<MaterialTypeDto>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.MaterialType.All.ToListAsync();
                return _mapper.Map<IEnumerable<MaterialTypeDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching MaterialType list: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.MaterialType.FindAsync(id) != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while checking MaterialType existence: {ex.Message}", ex);
            }
        }

        public async Task<MaterialTypeDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.MaterialType.FindAsync(id);
                return _mapper.Map<MaterialTypeDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching MaterialType by ID: {ex.Message}", ex);
            }
        }
    }
}