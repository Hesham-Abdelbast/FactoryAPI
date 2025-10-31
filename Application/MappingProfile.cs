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
               .ReverseMap();

            _ = CreateMap<Contact, ContactDto>()
               .ReverseMap();
            #endregion

        }
    }
}
