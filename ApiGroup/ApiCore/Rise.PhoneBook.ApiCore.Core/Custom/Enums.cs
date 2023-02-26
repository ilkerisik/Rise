using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ApiCore.Core.Custom
{
    public class Enums
    {
        public enum StatusEnum
        {
            [System.ComponentModel.Description("Hata Oluştu")]
            Error = 0,
            [System.ComponentModel.Description("Başarılı")]
            Successful = 1,
            [System.ComponentModel.Description("Uyarı")]
            Warning = 2,
            [System.ComponentModel.Description("Veri Yok")]
            EmptyData = 3
        }
        public enum ContactTypeEnum
        {
            [System.ComponentModel.Description("Telefon Numarası")]
            Phone = 1,
            [System.ComponentModel.Description(" E-mail Adresi")]
            Mail = 2,
            [System.ComponentModel.Description("Konum")]
            Location = 3,

        }
    }
}
