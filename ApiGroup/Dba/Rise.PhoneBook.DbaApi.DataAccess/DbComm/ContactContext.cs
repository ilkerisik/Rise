using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rise.PhoneBook.DbaApi.Entities.Concrete;

namespace Rise.PhoneBook.DbaApi.DataAccess.Concrete
{
    public partial class ContactContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
            IConfigurationRoot configuration = configBuilder.Build();
            string cnnStr = configuration.GetValue<string>("ConnectionStrings:Default");
            optionsBuilder.UseNpgsql(cnnStr);
        }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CreatedOn);
            });

            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactInfos)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("fk_contact");

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.ContactInfos)
                    .HasForeignKey(d => d.ContactTypeId)
                    .HasConstraintName("fk_contact_type");
            });

            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TypeName);

                entity.Property(e => e.TypeVal);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
