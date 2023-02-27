
using static Rise.PhoneBook.WebUI.Tools.Enums;

namespace Rise.PhoneBook.WebUI.Models.ApiModel
{
    public class StatusModel<T>
    {
        public T Entity { get; set; }
        public StatusModel Status { get; set; }
        public StatusModel()
        {
            Status = new StatusModel();
        }
    }


    public class StatusModel
    {
        private StatusEnum _status;

        public string Message { get; set; }
        public StatusEnum Status
        {
            get { return _status; }
            set
            {
                _status = value;
            }
        }
        public string Exception { get; set; }
    }
}
