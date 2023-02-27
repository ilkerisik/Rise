namespace Rise.PhoneBook.WebUI.Models.ApiModel
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
