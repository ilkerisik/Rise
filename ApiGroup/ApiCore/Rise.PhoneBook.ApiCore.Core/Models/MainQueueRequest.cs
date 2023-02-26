using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ApiCore.Core.Models
{
    public class MainQueueRequest
    {
        public string RequestId { get; set; }
        public string LocationFilter { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public string ApiUrl { get; set; }
    }
}
