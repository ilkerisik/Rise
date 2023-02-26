using Rise.PhoneBook.ApiCore.Core.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels
{
    public class ResLocationReportModel
    {
        public Entity[] entity { get; set; }
        public Status status { get; set; }

        public class Status
        {
            public string message { get; set; }
            public Enums.StatusEnum status { get; set; }
        }

        public class Entity
        {
            public string location { get; set; }
            public int personCount { get; set; }
            public int phoneCount { get; set; }
        }

    }
}
