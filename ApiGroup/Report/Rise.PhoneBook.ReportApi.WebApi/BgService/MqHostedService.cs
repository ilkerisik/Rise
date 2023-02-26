using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;

namespace Rise.PhoneBook.ReportApi.WebApi.BgService
{
    public class MqHostedService : BackgroundService
    {
        IQueueProcessorService _queueProcessorService;
        public MqHostedService(IQueueProcessorService queueProcessorService)
        {
            _queueProcessorService = queueProcessorService;
        }
        IModel channel;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            try
            {
                var factory = _queueProcessorService.CreateConnectionFactory();
                var conection = factory.CreateConnection();
                channel = conection.CreateModel();
                channel.QueueDeclare(queue: "Main", durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumerTemp = new AsyncEventingBasicConsumer(channel);
                consumerTemp.Received += ConsumerTemp_Received;
                channel.BasicConsume(queue: "Main", autoAck: true, consumer: consumerTemp);
            }
            catch (Exception ex)
            {
            }

            return Task.CompletedTask;
        }
        private async Task ConsumerTemp_Received(object sender, BasicDeliverEventArgs ea)
        {
            IModel model = channel;
            //Gelen Mesajların İşlenmesi
            string routingKey = ea.RoutingKey;
            model.BasicAck(ea.DeliveryTag, false);

        }
    }
}
