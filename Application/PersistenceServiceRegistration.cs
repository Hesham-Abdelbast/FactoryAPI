using DAL;
using DAL.Implementation;
using DAL.Implementation.Employees;
using DAL.Implementation.Equipments;
using DAL.Implementation.MerchantMangement;
using DAL.Implementation.Store;
using DAL.Interface;
using DAL.Interface.Employees;
using DAL.Interface.Equipments;
using DAL.Interface.MerchantMangement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Application
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            services.AddDatabase(config, env);
            services.AddIdentity(config);
            services.AddHEVORepos();
            services.RegisterSwagger();

            return services;
        }
        private static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Security API",
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
            });
        }
        private static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Basic API Info
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HEVO API",
                    Version = "v1",
                    Description = "API documentation for all E-commerce services with JWT authentication support."
                });

                // Define the Bearer JWT scheme
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token in the format: Bearer {your token}"
                });

                // Apply security requirement globally - FIXED VERSION
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            return services;
        }
        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            services.AddDbContext<FAContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("FactoryMangment_DB"))
                       .EnableThreadSafetyChecks()
                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
        {
            // Add Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("HEVO")
            .AddEntityFrameworkStores<FAContext>()
            .AddDefaultTokenProviders();

            // Add JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]))
                    };
                });

            services.AddAuthorization();

            return services;
        }
        private static IServiceCollection AddHEVORepos(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IMaterialTypeRepository, MaterialTypeRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();

            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantExpenseRepository, MerchantExpenseRepository>();

            services.AddScoped<IWarehouseRepositery, WarehouseRepositery>();
            services.AddScoped<IWarehouseInventoryRepo, WarehouseInventoryRepo>();
            services.AddScoped<IWarehouseExpenseRepository, WarehouseExpenseRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeCashAdvanceRepository, EmployeeCashAdvanceRepository>();
            services.AddScoped<IEmployeeMonthlyPayrollRepository, EmployeeMonthlyPayrollRepository>();
            services.AddScoped<IEmployeePersonalExpenseRepository, EmployeePersonalExpenseRepository>()
                ;
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IEquipmentExpenseRepository, EquipmentExpenseRepository>();
            services.AddScoped<IEquipmentIncomeRepository, EquipmentIncomeRepository>();

            services.AddScoped<IFinancingRepository, FinancingRepository>();
            services.AddScoped<IMerchantExpenseRepository, MerchantExpenseRepository>();
            return services;
        }
    }
}