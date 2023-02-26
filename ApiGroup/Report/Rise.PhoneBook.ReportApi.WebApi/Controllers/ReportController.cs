using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;

namespace Rise.PhoneBook.ReportApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportQueueProcessService _reportQueueProcessService;
        private readonly IConfiguration _config;
        public ReportController(IReportQueueProcessService reportQueueProcessService, IConfiguration config)
        {
            _reportQueueProcessService = reportQueueProcessService;
            _config = config;
        }

        [HttpGet("GetReportStatus/{requestId}")]
        public async Task<StatusModel<ResReportDetailModel>> GetReportStatus(Guid requestId)
        {
            var result = new StatusModel<ResReportDetailModel>();
            await Task.Factory.StartNew(() =>
            {
                
                result = _reportQueueProcessService.GetReportStatus(_config.GetValue<string>("DownloadApi:Url"),requestId);
            });
            return result;
        }

        [HttpGet("GetAllReportStatus")]
        public async Task<StatusModel<List<ResReportDetailModel>>> GetAllReportStatus()
        {
            var result = new StatusModel<List<ResReportDetailModel>>();
            await Task.Factory.StartNew(() =>
            {
                result = _reportQueueProcessService.GetAllReportStatus(_config.GetValue<string>("DownloadApi:Url"));
            });
            return result;
        }

        [HttpGet("DownloadReport/{fileId}")]
        public async Task<IActionResult> DownloadReport(Guid fileId)
        {
            var res = await GetReportStatus(fileId);
            if (res.Status.Status != Enums.StatusEnum.Successful ||
                res.Entity.DownloadUrl.IsNullOrEmpty())
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                string filePath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "//Content//" + fileId + ".xlsx";
                if (System.IO.File.Exists(filePath))
                {
                    var myBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(myBytes, "application/octet-stream", fileId + ".xlsx");
                }
            }
            return null;
        }

    }
}
