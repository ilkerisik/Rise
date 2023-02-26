using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rise.PhoneBook.ApiCore.Core.Entities;

namespace Rise.PhoneBook.DbaApi.Entities.Concrete
{
    [Table("contact_types")]
    public partial class ContactType : IEntity
    {
        public ContactType()
        {
            ContactInfos = new HashSet<ContactInfo>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("type_name")]
        [StringLength(20)]
        public string TypeName { get; set; }
        [Required]
        [Column("type_val")]
        [StringLength(50)]
        public string TypeVal { get; set; }

        [InverseProperty("ContactType")]
        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
    }
}
