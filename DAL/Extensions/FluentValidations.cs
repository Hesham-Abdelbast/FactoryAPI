using AppModels.Entities;
using AppModels.Entities.Drivers;
using AppModels.Entities.Employees;
using AppModels.Entities.Equipments;
using AppModels.Entities.MerchantMangement;
using AppModels.Entities.Store;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Extensions
{
    public static class FluentValidations
    {
        public static ModelBuilder AddFrameworkValidations(this ModelBuilder modelBuilder)
        {
            // Apply global query filters for soft delete
            modelBuilder.Entity<Merchant>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<MerchantExpense>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<MerchantFinance>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<MaterialType>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Transaction>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Contact>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Warehouse>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<WarehouseInventory>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<WarehouseExpense>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeMonthlyPayroll>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeePersonalExpense>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EmployeeCashAdvance>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Equipment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EquipmentExpense>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<EquipmentIncome>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Financing>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Driver>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<DriverExpense>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Travel>().HasQueryFilter(e => !e.IsDeleted);

            // تطبيق تلقائي لكل الـ entities اللي فيها IsDeleted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProp = entityType.ClrType.GetProperty("IsDeleted");
                if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, "IsDeleted");
                    var filter = Expression.Lambda(
                        Expression.Equal(prop, Expression.Constant(false)),
                        parameter
                    );

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

            // Configure unique constraints
            modelBuilder.Entity<MaterialType>()
                .HasIndex(m => m.Name)
                .IsUnique();

           

            // Configure relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Merchant)
                .WithMany(m => m.Transactions)
                .HasForeignKey(t => t.MerchantId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.MaterialType)
                .WithMany(m => m.Transactions)
                .HasForeignKey(t => t.MaterialTypeId);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
               .Property(t => t.Type)
               .HasConversion<int>();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.PricePerUnit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Warehouse)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);


            return modelBuilder;

        }
    }
}
