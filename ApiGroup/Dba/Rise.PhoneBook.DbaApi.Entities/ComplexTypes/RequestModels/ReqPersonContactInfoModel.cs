using Rise.PhoneBook.ApiCore.Core.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels
{
    public class ReqPersonContactInfoModel
    {
        public Guid PersonId { get; set; }
        public Guid? PersonContactId { get; set; }
        public Enums.ContactTypeEnum ContactType { get; set; }
        public string Info { get; set; }
    }
}
