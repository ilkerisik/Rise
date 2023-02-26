using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Newtonsoft.Json;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;
using System.Diagnostics;

namespace Rise.PhoneBook.ReportApi.TestProject
{
    public class MainUnitTest
    {
        string dbaApiUrl = "http://localhost:5236";

        [Fact]
        public void MainTest()
        {

        }
        [Fact]
        public async void QueueSendRandomLocation()
        {
            using (var httpClient = new HttpClient())
            {
                var location = DummyData.RandomCity();

                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/QueueProcess/QueueSend/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResQueueProcessorModel>>(apiResponse);
                    Debug.WriteLine(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }

        [Fact]
        public async void QueueSendAll()
        {
            using (var httpClient = new HttpClient())
            {
                var location = "ALL";
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/QueueProcess/QueueSend/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResQueueProcessorModel>>(apiResponse);
                    Debug.WriteLine(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }

        [Fact]
        public async void QueueSendRandomLocationMultiple()
        {
            for (int i = 0; i < 5; i++)
            {
                using (var httpClient = new HttpClient())
                {
                    var location = DummyData.RandomCity();

                    using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/QueueProcess/QueueSend/{location}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<StatusModel<ResQueueProcessorModel>>(apiResponse);
                        Debug.WriteLine(apiResponse);
                        Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                    }
                }
            }
        }
        [Fact]
        public async void GetReportStatus()
        {
            var requestId = "";
            using (var httpClient = new HttpClient())
            {
                var location = DummyData.RandomCity();

                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/QueueProcess/QueueSend/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResQueueProcessorModel>>(apiResponse);
                    Debug.WriteLine(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                    requestId = res.Entity.RequestId;
                }
            }
            bool isWhile = true;
            while (isWhile)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/GetReportStatus/{requestId}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<StatusModel<ResReportDetailModel>>(apiResponse);
                        Debug.WriteLine(apiResponse);
                        //Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                        if (res.Status.Status == Enums.StatusEnum.Successful && res.Entity.Status == "Tamamlandý")
                        {
                            isWhile = false;
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
        [Fact]
        public async void GetAllReportStatus()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/GetAllReportStatus"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<List<ResReportDetailModel>>>(apiResponse);
                    Debug.WriteLine(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }

        [Fact]
        public async void ReportDownload()
        {
            var fileId = "2c34dd7f-ca16-476b-bd4d-39f16a701bea";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/DownloadReport/{fileId}"))
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();
                    Assert.True(apiResponse.Length > 0);
                }
            }
        }
        [Fact]
        public async void ReportDownloadScenario()
        {
            var requestId = "";
            using (var httpClient = new HttpClient())
            {
                var location = DummyData.RandomCity();

                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/QueueProcess/QueueSend/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResQueueProcessorModel>>(apiResponse);
                    Debug.WriteLine(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                    requestId = res.Entity.RequestId;
                }
            }
            bool isWhile = true;
            while (isWhile)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/GetReportStatus/{requestId}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<StatusModel<ResReportDetailModel>>(apiResponse);
                        Debug.WriteLine(apiResponse);
                        //Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                        if (res.Status.Status == Enums.StatusEnum.Successful && res.Entity.Status == "Tamamlandý")
                        {
                            isWhile = false;
                        }
                    }
                }
                Thread.Sleep(1000);
            }
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/Report/DownloadReport/{requestId}"))
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();
                    Assert.True(apiResponse.Length > 0);
                }
            }
        }
    }
}