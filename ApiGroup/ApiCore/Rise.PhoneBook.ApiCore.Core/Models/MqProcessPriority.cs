using Rise.PhoneBook.ApiCore.Core.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ApiCore.Core.Models
{
    public class MqProcessPriority
    {
        public int OrderNo { get; set; }
        public string MqKey { get; set; }
        public Enums.StatusEnum Status { get; set; }
        public string Result { get; set; }
        public bool IsRunned { get; set; }
        public bool IsBreak { get; set; }
    }
}
