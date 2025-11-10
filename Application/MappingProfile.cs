using AppModels.Common;
using AppModels.Entities;
using AppModels.Models;
using AppModels.Models.Auth;
using AppModels.Models.Transaction;
using AutoMapper;
using DAL;
using Microsoft.IdentityModel.Tokens;

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

            #endregion

        }
    }
}
