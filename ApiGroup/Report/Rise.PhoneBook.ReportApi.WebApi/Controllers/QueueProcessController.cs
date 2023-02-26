using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ApiCore.Core.Models;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;

namespace Rise.PhoneBook.ReportApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueProcessController : ControllerBase
    {
        private readonly IQueueProcessorService _queueProcessorService;
        private readonly IConfiguration _config;
        public QueueProcessController(IQueueProcessorService queueProcessorService, IConfiguration config)
        {
            _queueProcessorService = queueProcessorService;
            _config = config;   
        }

        [HttpGet("QueueSend/{location}")]
        public StatusModel<ResQueueProcessorModel> QueueSend(string location)
        {
            //Sırası ile hangi Queues çalışacağını belirleyen bir yapı kurduk
            var q = new List<MqProcessPriority>() {
                        new MqProcessPriority() { OrderNo = 1, MqKey =Enums.QueueProcess.QReport.ToString(), Status = Enums.StatusEnum.Undefined },
                        new MqProcessPriority() { OrderNo = 2, MqKey =Enums.QueueProcess.QReportProcess.ToString(), Status = Enums.StatusEnum.Undefined },
                        new MqProcessPriority() { OrderNo = 3, MqKey =Enums.QueueProcess.QReportLastControl.ToString(), Status = Enums.StatusEnum.Undefined }
                    };
            var requestId = Guid.NewGuid().ToString();

            return _queueProcessorService.QueueSend(new MainQueueRequest()
            {
                RequestId = requestId,
                LocationFilter = location,
                ApiUrl = _config.GetValue<string>("ReportApi:Url")
            }, q, Enums.QueueProcess.QReport);
        }
    }
}
