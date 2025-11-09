using AppModels.Entities;
using AppModels.Models;
using AppModels.Models.Auth;
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

            _ = CreateMap<TransactionDto, Transaction>()
               .ReverseMap()
               .ForMember(dest => dest.MaterialTypeName, opt => opt.MapFrom(x => x.MaterialType.Name))
               .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(x => x.Merchant.Name));

            _ = CreateMap<Contact, ContactDto>()
               .ReverseMap();

            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
            #endregion

        }
    }
}
