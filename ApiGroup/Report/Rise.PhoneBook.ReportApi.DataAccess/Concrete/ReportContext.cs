using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rise.PhoneBook.ReportApi.Entities.Concrete;

namespace Rise.PhoneBook.ReportApi.DataAccess.Concrete
{
    public partial class ReportContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
            IConfigurationRoot configuration = configBuilder.Build();
            string cnnStr = configuration.GetValue<string>("ConnectionStrings:Default");
            optionsBuilder.UseNpgsql(cnnStr);
        }
        public virtual DbSet<ReportQueueProcess> ReportQueueProcesses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportQueueProcess>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
