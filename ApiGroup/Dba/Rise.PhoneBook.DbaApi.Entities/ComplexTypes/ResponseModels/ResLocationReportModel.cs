using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels
{
    public class ResLocationReportModel
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
