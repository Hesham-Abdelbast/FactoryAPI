using AppModels.Common;
using AppModels.Entities;
using AppModels.Entities.Employees;
using AppModels.Entities.Equipments;
using AppModels.Entities.MerchantMangement;
using AppModels.Entities.Store;
using AppModels.Models;
using AppModels.Models.Auth;
using AppModels.Models.Employees;
using AppModels.Models.Equipments;
using AppModels.Models.MerchantMangement;
using AppModels.Models.Store;
using AppModels.Models.Transaction;
using AutoMapper;
using DAL;

namespace Application
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Common



            #endregion

            #region
            _ = CreateMap<ApplicationUser, RegistrationVM>()
                .ReverseMap();

            _ = CreateMap<MaterialTypeDto, MaterialType>()
                .ReverseMap();

            _ = CreateMap<MerchantDto, Merchant>()
                .ReverseMap();

            _ = CreateMap<MerchantExpenseDto, MerchantExpense>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Notes))
                .ReverseMap()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(x => x.Description));

            _ = CreateMap<MerchantExpenseCreateDto, MerchantExpense>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Notes))
                .ReverseMap()
                .ForMember(dest => dest.Notes , opt => opt.MapFrom(x=>x.Description));

            _ = CreateMap<Contact, ContactDto>()
               .ReverseMap();

            _ = CreateMap<Warehouse, WarehouseDto>().ReverseMap();


            _ = CreateMap<TransactionDto, Transaction>()
               .ReverseMap()
               .ForMember(dest => dest.MaterialTypeName, opt => opt.MapFrom(x => x.MaterialType.Name))
               .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(x => x.Merchant.Name))
               .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(x => x.Warehouse.Name))
               .ForMember(dest => dest.TypeNameAr, opt => opt.MapFrom(x => x.Type ==TransactionType.Income ? "وارد":"صادر"));

            _ = CreateMap<CreateTransactionDto, Transaction>()
               .ReverseMap();

            _ = CreateMap<WarehouseInventoryDto, WarehouseInventory>()
                .ReverseMap()
                .ForMember(dest => dest.MaterialTypeName, opt => opt.MapFrom(x => x.MaterialType.Name))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(x => x.Warehouse.Name));

            _ = CreateMap<WarehouseExpenseDto, WarehouseExpense>()
                .ReverseMap()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(x => x.Warehouse.Name));

            _ = CreateMap<Employee, EmployeeDto>().ReverseMap();

            _ = CreateMap<EmployeePersonalExpense, EmployeePersonalExpenseDto>().ReverseMap();

            _ = CreateMap<EmployeeCashAdvance, EmployeeCashAdvanceDto>().ReverseMap();

            _ = CreateMap<EmployeeMonthlyPayroll, EmployeeMonthlyPayrollDto>().ReverseMap();


            _ = CreateMap<EquipmentIncomeDto, EquipmentIncome>().ReverseMap()
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(x => x.Equipment.Name));

            _ = CreateMap<EquipmentExpenseDto, EquipmentExpense>().ReverseMap()
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(x => x.Equipment.Name));

            _ = CreateMap<EquipmentDto, Equipment>().ReverseMap();

            _ = CreateMap<FinancingDto, Financing>().ReverseMap();
            _ = CreateMap<FinancingCreateDto, Financing>().ReverseMap();

            #endregion

        }
    }
}