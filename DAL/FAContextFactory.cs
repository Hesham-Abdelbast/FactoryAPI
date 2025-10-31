using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class FAContextFactory : IDesignTimeDbContextFactory<FAContext>
    {
        public FAContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FAContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=FactoryMangment;Trusted_Connection=True;TrustServerCertificate=True;");

            return new FAContext(optionsBuilder.Options);
        }
    }
}
