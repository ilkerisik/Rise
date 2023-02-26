using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rise.PhoneBook.ApiCore.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ReportApi.Entities.Concrete
{
    [Table("report_queue_processes", Schema = "report")]
    public partial class ReportQueueProcess : IEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("datas", TypeName = "jsonb")]
        public string? Datas { get; set; }

        [Column("queueprocess", TypeName = "jsonb")]
        public string? Queueprocess { get; set; }
        [Column("last_queue_name")]
        [StringLength(255)]
        public string LastQueueName { get; set; }
        [Column("queue_status")]
        [StringLength(255)]
        public string QueueStatus { get; set; }
        [Column("filename")]
        [StringLength(255)]
        public string Filename { get; set; }
        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
        [Column("changed_on")]
        public DateTime? ChangedOn { get; set; }
        [Column("state")]
        public short? State { get; set; }
    }
}
