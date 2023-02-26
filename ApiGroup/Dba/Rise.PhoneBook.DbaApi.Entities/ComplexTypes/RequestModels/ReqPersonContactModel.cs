using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels
{
    public class ReqPersonContactModel
    {
        /// <summary>
        /// Kişi için isteğe bağlı UUID değeri girebilirsiniz.
        /// </summary>
        public Guid? PersonId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Company { get; set; } = default;
    }
}
