using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels
{
    public class ResReportDetailModel
    {
        public Guid RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReportCreateDate { get; set; }
        public string DownloadUrl { get; set; }
        public string Status { get; set; }
    }
}
