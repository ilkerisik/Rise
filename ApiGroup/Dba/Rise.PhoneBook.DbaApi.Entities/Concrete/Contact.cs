using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rise.PhoneBook.ApiCore.Core.Entities;

namespace Rise.PhoneBook.DbaApi.Entities.Concrete
{
    [Table("contacts")]
    public partial class Contact : IEntity
    {
        public Contact()
        {
            ContactInfos = new HashSet<ContactInfo>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("firstname")]
        [StringLength(40)]
        public string Firstname { get; set; }
        [Required]
        [Column("lastname")]
        [StringLength(20)]
        public string Lastname { get; set; }
        [Column("company")]
        [StringLength(350)]
        public string? Company { get; set; } = default;

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [InverseProperty("Contact")]
        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
    }
}
