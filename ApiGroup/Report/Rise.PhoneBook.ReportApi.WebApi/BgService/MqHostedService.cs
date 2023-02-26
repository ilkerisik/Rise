using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ApiCore.Core.Models;
using Rise.PhoneBook.ReportApi.Business.Abstract;

namespace Rise.PhoneBook.ReportApi.WebApi.BgService
{
    public class MqHostedService : BackgroundService
    {
        IReportQueueProcessService _reportQueueProcessService;
        IQueueProcessorService _queueProcessorService;

        List<string> _strProcess;
        public MqHostedService(IReportQueueProcessService reportQueueProcessService, IQueueProcessorService queueProcessorService)
        {
            _reportQueueProcessService = reportQueueProcessService;
            _queueProcessorService = queueProcessorService;
            //Tüm Adımlar için Kuyruk Dinleyicileri
            _strProcess = ExtensionMethods.EnumListOf<Enums.QueueProcess>();
        }
        IModel channel;
        public Dictionary<string, IModel> ModelMq = new Dictionary<string, IModel>();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            try
            {
                var factory = _queueProcessorService.CreateConnectionFactory();
                var conection = factory.CreateConnection();
                foreach (var item in _strProcess)
                {
                    channel = conection.CreateModel();
                    channel.QueueDeclare(queue: item, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    var consumerTemp = new AsyncEventingBasicConsumer(channel);
                    consumerTemp.Received += ConsumerTemp_Received;
                    channel.BasicConsume(queue: item, autoAck: false, consumer: consumerTemp);
                    ModelMq.Add(item, channel);
                }
            }
            catch (Exception ex)
            {
            }

            return Task.CompletedTask;
        }
        private async Task ConsumerTemp_Received(object sender, BasicDeliverEventArgs ea)
        {
            IModel model = channel;
            string routingKey = "--";
            string eaBasicPropertiesMessageId = "--";
            try
            {
                #region MyRegion
                routingKey = ea.RoutingKey;
                model = ModelMq[routingKey];
                var receiverProcess = (Enums.QueueProcess)System.Enum.Parse(typeof(Enums.QueueProcess), routingKey);
                List<MqProcessPriority> header_QueueProcess = ea.BasicProperties.Headers[Enums.MqHeaders.MqQueueProcess.ToString()].ToByteArray().ToByteArrayUtf8String().FromJson<List<MqProcessPriority>>();

                var requestId = "";
                if (ea.BasicProperties.Headers.ContainsKey(Enums.MqHeaders.RequestId.ToString()))
                    requestId = ea.BasicProperties.Headers[Enums.MqHeaders.RequestId.ToString()].ToByteArray().ToByteArrayUtf8String();
                var sendData = ea.Body.ToArray();
                string filePath = "";
                var saveStatus = _reportQueueProcessService.QueueProcessAdd(requestId, sendData, header_QueueProcess, Enums.StatusEnum.Successful, routingKey, filePath);

                if (ea.BasicProperties.MessageId != null)
                    eaBasicPropertiesMessageId = ea.BasicProperties.MessageId;
                switch (receiverProcess)
                {
                    case Enums.QueueProcess.QReport:
                        var resMainProcess = _reportQueueProcessService.QReport(sendData, ref header_QueueProcess);
                        if (resMainProcess.Status.Status == Enums.StatusEnum.Successful)
                        {
                            sendData = resMainProcess.Entity;
                        }
                        break;
                    case Enums.QueueProcess.QReportProcess:
                        var resFileSaveProcess = _reportQueueProcessService.QFile(sendData, ref header_QueueProcess);
                        if (resFileSaveProcess.Status.Status == Enums.StatusEnum.Successful)
                        {
                            filePath = resFileSaveProcess.Status.Message;
                            sendData = resFileSaveProcess.Entity;
                        }
                        break;
                    case Enums.QueueProcess.QReportLastControl:
                        var resLastControlProcess = _reportQueueProcessService.QReportLast(sendData, ref header_QueueProcess);
                        if (resLastControlProcess.Status.Status == Enums.StatusEnum.Successful)
                            sendData = null;
                        break;
                    default:
                        break;
                }
                saveStatus = _reportQueueProcessService.QueueProcessAdd(requestId, sendData, header_QueueProcess, Enums.StatusEnum.Successful, routingKey, filePath);

                bool isDataEmpty = true;
                int sendRouteNo = 0;
                string sendRoute = "";
                foreach (var item in header_QueueProcess.OrderBy(i => i.OrderNo))
                {
                    if (item.IsBreak)
                    {
                        isDataEmpty = false;
                        break;
                    }
                    else if (item.OrderNo > sendRouteNo && item.Status == Enums.StatusEnum.Undefined)
                    {
                        sendRouteNo = item.OrderNo;
                        sendRoute = item.MqKey;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(sendRoute))
                {
                    var PropertiesMq = model.CreateBasicProperties();
                    PropertiesMq.Persistent = true;
                    PropertiesMq.CorrelationId = Guid.Empty.ToString();
                    PropertiesMq.MessageId = ea.BasicProperties.MessageId;
                    PropertiesMq.Headers = new Dictionary<string, object>();
                    PropertiesMq.Headers.Add(Enums.MqHeaders.MqQueueProcess.ToString(), header_QueueProcess.ToJson());
                    if (!string.IsNullOrEmpty(requestId))
                        PropertiesMq.Headers.Add(Enums.MqHeaders.RequestId.ToString(), requestId);

                    model.BasicPublish(exchange: "", routingKey: sendRoute,
                    basicProperties: PropertiesMq, body: sendData);
                }
                else
                {
                    saveStatus = _reportQueueProcessService.QueueProcessAdd(requestId, (isDataEmpty ? null : sendData), header_QueueProcess, Enums.StatusEnum.Successful, routingKey, filePath);
                }
                #endregion
                model.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
            }

        }
    }
}
