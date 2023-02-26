using RabbitMQ.Client;
using Rise.PhoneBook.ApiCore.Core.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ReportApi.Business.Abstract
{
    public interface IQueueProcessorService
    {
        ConnectionFactory CreateConnectionFactory();
        StatusModel<string> QueueSend(Guid requestId, string location);
    }
}
