using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ApiCore.Core.Custom
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
        private Enums.StatusEnum _status;

        public string Message { get; set; }
        public Enums.StatusEnum Status
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
