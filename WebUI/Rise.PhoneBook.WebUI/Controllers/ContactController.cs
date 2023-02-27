using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rise.PhoneBook.WebUI.Models;
using Rise.PhoneBook.WebUI.Models.ApiModel;
using System.Text;

namespace Rise.PhoneBook.WebUI.Controllers
{
    public class ContactController : Controller
    {
        string dbaApiUrl = "";

        private readonly IConfiguration _config;
        public ContactController(IConfiguration config)
        {
            _config = config;
            dbaApiUrl = _config.GetValue<string>("DbaApi:Url");
        }

        public async Task<IActionResult> Index()
        {
            var res = new List<ResPersonContactModel>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetAllPersonList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var resApi = JsonConvert.DeserializeObject<StatusModel<IList<ResPersonContactModel>>>(apiResponse);
                    if (resApi != null && resApi.Status.Status == Tools.Enums.StatusEnum.Successful)
                    {
                        res = resApi.Entity.ToList();
                    }
                }
            }

            return View(res);
        }

        public IActionResult Create(CreatePersonApiModel data)
        {
            data.personId = Guid.NewGuid().ToString();
            return View(data);
        }
        public IActionResult CreateInfo(Guid personId)
        {
            var data = new ReqPersonContactInfoModel()
            {
                PersonId = personId,
                PersonContactId = Guid.NewGuid()
            };
            return View(data);
        }

        public async Task<IActionResult> DeleteInfo(Guid personId, Guid personContactId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{dbaApiUrl}/api/MainContact/Contact/DeleteInfoById/{personContactId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse);
                    return Redirect("/Contact/PersonInfo?personId=" + personId);
                }
            }
        }
        public async Task<IActionResult> PersonInfo(Guid personId)
        {
            var res = new ResAllPersonInfo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetPersonByAllInfoList/{personId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        var resApi = JsonConvert.DeserializeObject<StatusModel<ResAllPersonInfo>>(apiResponse);
                        if (resApi != null)
                        {
                            res = resApi.Entity;
                        }
                    }
                }
            }
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel(CreatePersonApiModel data)
        {
            if (ModelState.IsValid)
            {
                var res = new ActionResultModel
                {
                    ActionStatus = Tools.Enums.StatusEnum.Warning.ToString(),
                    ActionTitle = "Kayıt İşlemi",
                    ActionMessage = "Hata",
                    ReturnUrl = "/Contact"
                };
                using (var httpClient = new HttpClient())
                {
                    var httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreatePerson", httpContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var resApi = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);
                        if (resApi != null)
                        {
                            res.ActionStatus = resApi.Status.Status.ToString();
                            res.ActionMessage = resApi.Status.Message;
                        }
                    }
                }
                return Json(res);
            }
            return Json(new ActionResultModel
            {
                ActionStatus = Tools.Enums.StatusEnum.Warning.ToString(),
                ActionTitle = "Kayıt İşlemi Yapılamadı",
                ActionMessage = "Lütfen değerleri kontrol ediniz."
            });

        }
        [HttpPost]
        public async Task<IActionResult> CreateInfoModel(ReqPersonContactInfoModel data)
        {
            if (ModelState.IsValid)
            {
                var res = new ActionResultModel
                {
                    ActionStatus = Tools.Enums.StatusEnum.Warning.ToString(),
                    ActionTitle = "Kayıt İşlemi",
                    ActionMessage = "Hata",
                    ReturnUrl = "/Contact"
                };

                using (var httpClient2 = new HttpClient())
                {
                    var httpContent2 = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    using (var response2 = await httpClient2.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreateInfoToPerson", httpContent2))
                    {
                        string apiResponse2 = await response2.Content.ReadAsStringAsync();
                        var resApi = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse2);
                        if (resApi != null)
                        {
                            res.ActionStatus = resApi.Status.Status.ToString();
                            res.ActionMessage = resApi.Status.Message;
                        }
                    }
                }

                return Json(res);
            }
            return Json(new ActionResultModel
            {
                ActionStatus = Tools.Enums.StatusEnum.Warning.ToString(),
                ActionTitle = "Kayıt İşlemi Yapılamadı",
                ActionMessage = "Lütfen değerleri kontrol ediniz."
            });

        }

    }
}
