namespace Rise.PhoneBook.WebUI.Models.ApiModel
{
    public class ResPersonContactModel
    {
        public Guid PersonId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Company { get; set; } = default;
        public DateTime CreatedOn { get; set; }
    }

}
