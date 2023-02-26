using RabbitMQ.Client;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Microsoft.Extensions.Configuration;
using System.Text;
using Rise.PhoneBook.ApiCore.Core.Models;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;

namespace Rise.PhoneBook.ReportApi.Business.Concrete
{
    public class QueueProcessorService : IQueueProcessorService
    {
        private readonly IConfiguration _config;
        public QueueProcessorService(IConfiguration config)
        {
            _config = config;
        }
        public ConnectionFactory CreateConnectionFactory()
        {
            var factory = new ConnectionFactory() { HostName = _config.GetValue<string>("RabbitMqSetting:Host") };
            factory.UserName = _config.GetValue<string>("RabbitMqSetting:User");
            factory.Password = _config.GetValue<string>("RabbitMqSetting:Pass");
            factory.VirtualHost = "/";
            factory.HostName = _config.GetValue<string>("RabbitMqSetting:Host");
            factory.DispatchConsumersAsync = true;
            factory.RequestedHeartbeat = new TimeSpan(0, 0, 90);
            return factory;
        }
        public StatusModel<ResQueueProcessorModel> QueueSend(MainQueueRequest req, List<MqProcessPriority> mqQueueProcess, Enums.QueueProcess routingKey)
        {
            StatusModel<ResQueueProcessorModel> result = new StatusModel<ResQueueProcessorModel>() { Status = new StatusModel() { } };
            try
            {
                var factory = CreateConnectionFactory();
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.CorrelationId = Guid.Empty.ToString();
                        properties.MessageId = req.RequestId;
                        properties.Headers = new Dictionary<string, object>();
                        properties.Headers.Add(Enums.MqHeaders.RequestId.ToString(), req.RequestId);
                        properties.Headers.Add(Enums.MqHeaders.MqQueueProcess.ToString(), mqQueueProcess.ToJson());
                        channel.BasicPublish(exchange: "", routingKey: routingKey.ToString(), basicProperties: properties, body: req.ToJsonByteArray());
                    }
                }
                result.Entity = new ResQueueProcessorModel() { IsSend = true, RequestId = req.RequestId };
                result.Status.Status = Enums.StatusEnum.Successful;
                result.Status.Message = "Talep Oluşturuldu";
            }
            catch (Exception ex)
            {
                result.Entity = new ResQueueProcessorModel() { IsSend = false };
                result.Status.Status = Enums.StatusEnum.Error;
                result.Status.Message = ex.Message;
            }
            return result;
        }

    }
}
