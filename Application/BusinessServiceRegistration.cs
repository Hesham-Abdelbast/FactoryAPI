using Application.Implementation;
using Application.Implementation.Auth;
using Application.Interface;
using Application.Interface.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

            services.AddHEVOServices();

            return services;
        }

        private static IServiceCollection AddHEVOServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<ITokenServices, TokenServices>();

            services.AddScoped<IFileServices, FileServices>();

            services.AddScoped<IMaterialTypeServices, MaterialTypeServices>();
            services.AddScoped<IStoreInventoryServices, StoreInventoryServices>();
            services.AddScoped<IMerchantServices, MerchantServices>();
            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddScoped<IContactServices, ContactServices>();
            services.AddScoped<IWarehouseServices,WarehouseServices>();


            return services;
        }
    }
}
