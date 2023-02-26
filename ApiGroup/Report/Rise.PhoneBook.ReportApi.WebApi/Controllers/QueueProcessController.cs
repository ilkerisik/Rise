using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Rise.PhoneBook.ReportApi.Business.Concrete;

namespace Rise.PhoneBook.ReportApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueProcessController : ControllerBase
    {
        private readonly IQueueProcessorService _queueProcessorService;
        public QueueProcessController(IQueueProcessorService queueProcessorService)
        {
            _queueProcessorService = queueProcessorService;
        }

        [HttpGet("QueueSend/{location}")]
        public StatusModel<string> QueueSend(string location)
        {
            var requestId = Guid.NewGuid();
            return _queueProcessorService.QueueSend(requestId,location);
        }
    }
}
