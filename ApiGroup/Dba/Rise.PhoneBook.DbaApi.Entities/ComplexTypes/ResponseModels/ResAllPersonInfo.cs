using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels
{
    public class ResAllPersonInfo : ResPersonContactModel
    {
        public List<ResPersonContactInfoModel> ContactInfoList { get; set; }
    }
}
