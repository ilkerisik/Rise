using RabbitMQ.Client;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Microsoft.Extensions.Configuration;
using System.Text;

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

        public StatusModel<string> QueueSend(Guid requestId, string location)
        {
            StatusModel<string> result = new StatusModel<string>() { Status = new StatusModel() { } };
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
                        properties.MessageId = requestId.ToString();
                        properties.Headers = new Dictionary<string, object>();
                        channel.BasicPublish(exchange: "", routingKey: "Main".ToString(), basicProperties: properties, body: Encoding.UTF8.GetBytes(location));
                    }
                }
                result.Entity = requestId.ToString();
                result.Status.Status = Enums.StatusEnum.Successful;
                result.Status.Message = "Talep Oluşturuldu";
            }
            catch (Exception ex)
            {
                result.Entity = "";
                result.Status.Status = Enums.StatusEnum.Error;
                result.Status.Message = ex.Message;
            }
            return result;
        }

    }
}
