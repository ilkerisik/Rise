using RabbitMQ.Client;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ApiCore.Core.Models;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;
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
        StatusModel<ResQueueProcessorModel> QueueSend(MainQueueRequest req, List<MqProcessPriority> mqQueueProcess, Enums.QueueProcess routingKey);
    }
}
