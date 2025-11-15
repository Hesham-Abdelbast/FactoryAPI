using AppModels.Common;
using AppModels.Entities;
using AppModels.Entities.Employees;
using AppModels.Entities.Equipments;
using AppModels.Entities.MerchantMangement;
using AppModels.Entities.Store;
using DAL.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DAL
{
    public partial class FAContext : IdentityDbContext<ApplicationUser>
    {
        public FAContext()
        {
        }
        public FAContext(DbContextOptions<FAContext> options)
       : base(options)
        {
        }

        /// <summary>
        /// DbSet for merchants
        /// </summary>
        public DbSet<Merchant> Merchants { get; set; }

        /// <summary>
        /// DbSet for material types
        /// </summary>
        public DbSet<MaterialType> MaterialTypes { get; set; }

        /// <summary>
        /// DbSet for transactions
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }


        /// <summary>
        /// DbSet for warehouses
        /// </summary>
        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Financing> Financing { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddFrameworkValidations();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateDate = DateTime.UtcNow;
                        //entry.Entity.CreatedBy = 1;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdateDate = DateTime.UtcNow;
                        //entry.Entity.UpdatedBy = 1;

                        break;
                }
            }
            return base.SaveChanges();
        }
    }
}
