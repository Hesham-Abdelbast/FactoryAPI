using Application.Implementation;
using Application.Implementation.Auth;
using Application.Implementation.Drivers;
using Application.Implementation.Employees;
using Application.Implementation.Equipments;
using Application.Implementation.MerchantMangement;
using Application.Implementation.Store;
using Application.Implementation.SystemInventory;
using Application.Interface;
using Application.Interface.Auth;
using Application.Interface.Drivers;
using Application.Interface.Employees;
using Application.Interface.Equipments;
using Application.Interface.MerchantMangement;
using Application.Interface.Store;
using Application.Interface.SystemInventory;
using Application.Services;
using AppModels.Entities.Store;
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

            services.AddScoped<IMerchantServices, MerchantServices>();
            services.AddScoped<IMerchantExpenseService, MerchantExpenseService>();

            services.AddScoped<IMaterialTypeServices, MaterialTypeServices>();
            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddScoped<IContactServices, ContactServices>();
            services.AddScoped<IWarehouseServices,WarehouseServices>();
            services.AddScoped<IWarehouseExpenseServices, WarehouseExpenseServices>();
            services.AddScoped<IWarehouseInventoryServices, WarehouseInventoryServices>();

            services.AddScoped<IFinancingService, FinancingService>();
            services.AddScoped<IEmployeeManagementService,EmployeeManagementService>();
            services.AddScoped<IEquipmentManagementService,EquipmentManagementService>();
            services.AddScoped<ISystemInventoryServices, SystemInventoryServices>();

            services.AddScoped<IDriverManagementService, DriverManagementService>();

            return services;
        }
    }
}
