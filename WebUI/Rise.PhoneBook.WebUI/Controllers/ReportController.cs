using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rise.PhoneBook.WebUI.Models;
using Rise.PhoneBook.WebUI.Models.ApiModel;
using Rise.PhoneBook.WebUI.Tools;

namespace Rise.PhoneBook.WebUI.Controllers
{
    public class ReportController : Controller
    {
        string dbaApiUrl = "";

        private readonly IConfiguration _config;
        public ReportController(IConfiguration config)
        {
            _config = config;
            dbaApiUrl = _config.GetValue<string>("ReportApi:Url");
        }
        public async Task<IActionResult> Index()
        {
            var res = new List<ResReportDetailModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/GetAllReportStatus"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var resApi = JsonConvert.DeserializeObject<StatusModel<List<ResReportDetailModel>>>(apiResponse);
                    if (resApi != null && resApi.Status.Status == Enums.StatusEnum.Successful)
                    {
                        res = resApi.Entity.ToList();
                    }
                }
            }

            return View(res);
        }

        public async Task<IActionResult> Request()
        {
            return View();
        }

        public async Task<IActionResult> ReportDetail(Guid requestId)
        {
            var res = new ResReportDetailModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/GetReportStatus/{requestId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var resApi = JsonConvert.DeserializeObject<StatusModel<ResReportDetailModel>>(apiResponse);
                    if (resApi != null && resApi.Status.Status == Enums.StatusEnum.Successful)
                    {
                        res = resApi.Entity;
                    }
                }
            }
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> ReportRequest(string location)
        {
            var res = new ActionResultModel
            {
                ActionStatus = Tools.Enums.StatusEnum.Warning.ToString(),
                ActionTitle = "Rapor Talebi",
                ActionMessage = "Hata",
                ReturnUrl = "/Request"
            };
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/QueueProcess/QueueSend/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var resApi = JsonConvert.DeserializeObject<StatusModel<ResQueueProcessorModel>>(apiResponse);
                    if (resApi != null)
                    {
                        res.ActionStatus = resApi.Status.Status.ToString();
                        res.ActionMessage = resApi.Entity.IsSend + " / " + resApi.Entity.RequestId;
                        res.ReturnUrl = "/Report/ReportDetail?requestId=" + resApi.Entity.RequestId;
                        //return Redirect(res.ReturnUrl);
                    }
                }
            }
            return Json(res);
        }
    }
}
