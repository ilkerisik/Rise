namespace Rise.PhoneBook.WebUI.Models.ApiModel
{
    public class ResPersonContactInfoModel
    {
        public Guid PersonId { get; set; }
        public Guid PersonContactId { get; set; }
        public Rise.PhoneBook.WebUI.Tools.Enums.ContactTypeEnum ContactType { get; set; }
        public string Info { get; set; }
    }
}
