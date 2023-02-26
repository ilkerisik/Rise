using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rise.PhoneBook.ApiCore.Core.Entities;

namespace Rise.PhoneBook.DbaApi.Entities.Concrete
{
    [Table("contact_infos")]
    public partial class ContactInfo : IEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("contact_id")]
        public Guid ContactId { get; set; }
        [Column("contact_type_id")]
        public int ContactTypeId { get; set; }
        [Required]
        [Column("info")]
        [StringLength(500)]
        public string Info { get; set; }
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("ContactInfos")]
        public virtual Contact Contact { get; set; }
        [ForeignKey("ContactTypeId")]
        [InverseProperty("ContactInfos")]
        public virtual ContactType ContactType { get; set; }
    }
}
