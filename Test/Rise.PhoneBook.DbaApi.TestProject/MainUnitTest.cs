using Newtonsoft.Json;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.DataAccess.Concrete;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.DbaApi.Entities.Concrete;
using System;
using System.Text;

namespace Rise.PhoneBook.DbaApi.TestProject
{
    public class MainUnitTest
    {
        string dbaApiUrl = "http://localhost:5206";
        #region Doðrudan Veritabaný Eriþim Ýþlemleri
        [Fact]
        public void DbContactAddTest()
        {
            using (var c = new ContactContext())
            {
                var data = c.Contacts.Add(new Entities.Concrete.Contact()
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Ýlker",
                    Lastname = "IÞIK",
                    Company = "IÞIK A.Þ",
                    CreatedOn = DateTime.UtcNow
                });
                int res = c.SaveChanges();
                Assert.True(res > 0);
            }
        }

        [Fact]
        public void DbContactGetAllTest()
        {
            using (var c = new ContactContext())
            {
                var data = c.Contacts.Where(i => true);
                Assert.True(data.Any());
            }
        }
        #endregion

        #region Api Eriþim Testi
        [Fact]
        public async void GetAllPersonList()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<IList<Contact>>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                    Assert.False(res.Status.Status == Enums.StatusEnum.Error);
                }
            }
        }
        #endregion
        #region Kiþi Ekleme Ýþlemleri
        #region Unique Veriler ile Tam Ýçerik Gönderme Testi
        [Fact]
        public async void CreatePerson1()
        {
            using (var httpClient = new HttpClient())
            {
                //DummyData ile Tam veri  Oluþturma
                var dataJson = new ReqPersonContactModel()
                {
                    PersonId = Guid.NewGuid(), //Her zaman yeni üretildiði için eklenmesi beklenir
                    Firstname = DummyData.RandomFirstName(),
                    Lastname = DummyData.RandomLastname(),
                    Company = DummyData.RandomCompanyName(),
                };

                var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreatePerson", httpContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }
        #endregion
        #region Ýlk veri olmayan ikinci veri önce eklenen veri olacak
        [Fact]
        public async void CreatePerson2()
        {
            //Ýlk Ýþlem Baþarýlý olarak beklenmekte
            using (var httpClient = new HttpClient())
            {
                //DummyData ile Company boþ veri  Oluþturma
                var dataJson = new ReqPersonContactModel()
                {
                    PersonId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Firstname = DummyData.RandomFirstName(),
                    Lastname = DummyData.RandomLastname()
                };

                var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreatePerson", httpContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }

            //Ýkinci veri Daha Önce Eklendi
            using (var httpClient = new HttpClient())
            {
                //DummyData ile Tam veri  Oluþturma
                var dataJson = new ReqPersonContactModel()
                {
                    PersonId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Firstname = DummyData.RandomFirstName(),
                    Lastname = DummyData.RandomLastname(),
                    Company = DummyData.RandomCompanyName(),
                };

                var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreatePerson", httpContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);
                    //Eklemek istediðiniz id sistemde tanýmlý!
                    Assert.True(res.Status.Status == Enums.StatusEnum.Warning);
                }
            }
        }
        #endregion 
        #endregion


        #region Kiþi ye Bilgi Ekleme Ýþlemleri
        #region Unique Veriler ile Tam Ýçerik Gönderme Testi
        [Fact]
        public async void CreatePersonInfoByLocation1()
        {
            using (var httpClient = new HttpClient())
            {
                #region Bilgi olarak Rasgele Lokasyon Ekle
                var typ = Enums.ContactTypeEnum.Location;
                //DummyData ile Tam veri  Oluþturma
                var dataJson = new ResPersonContactInfoModel()
                {
                    PersonId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    PersonContactId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    ContactType = typ,
                    Info = DummyData.RandomContactTypeByInfo(typ)
                };

                var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreateInfoToPerson", httpContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
                #endregion
            }
        }
        [Fact]
        public async void CreatePersonInfoByPhone()
        {
            using (var httpClient = new HttpClient())
            {
                for (int i = 0; i < 3; i++)
                {
                    #region Bilgi olarak Rasgele Telefon Ekle
                    var typ = Enums.ContactTypeEnum.Phone;
                    //DummyData ile Tam veri  Oluþturma
                    var dataJson = new ResPersonContactInfoModel()
                    {
                        PersonId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                        PersonContactId = Guid.NewGuid(),
                        ContactType = typ,
                        Info = DummyData.RandomContactTypeByInfo(typ)
                    };

                    var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreateInfoToPerson", httpContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse);
                        Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                    }
                    #endregion
                }
            }
        }
        #endregion
        #region Ýlk veri olmayan ikinci veri önce eklenen veri olacak
        [Fact]
        public async void CreateAllPersonToInfoCreate()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetAllPersonList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<IList<ResPersonContactModel>>>(apiResponse);

                    if (res.Status.Status == Enums.StatusEnum.Successful)
                        foreach (var item in res.Entity)
                        {
                            using (var httpClient2 = new HttpClient())
                            {
                                var typ = Enums.ContactTypeEnum.Phone;// DummyData.RandomContactType();
                                //DummyData ile Tam veri  Oluþturma
                                var dataJson = new ReqPersonContactInfoModel()
                                {
                                    PersonId = item.PersonId,
                                    PersonContactId = Guid.NewGuid(),
                                    ContactType = typ,
                                    Info = DummyData.RandomContactTypeByInfo(typ)
                                };
                                var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                                using (var response2 = await httpClient2.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreateInfoToPerson", httpContent))
                                {
                                    string apiResponse2 = await response2.Content.ReadAsStringAsync();
                                    var res2 = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse2);
                                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                                }
                            }
                        }


                }
            }

        }
        #endregion
        #endregion

        #region Tüm Kiþileri Listele
        [Fact]
        public async void GetAllPersonList2()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetAllPersonList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<IList<ResPersonContactModel>>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }
        #endregion

        #region Kiþiler ve Tüm Bilgilerini Listele
        [Fact]
        public async void GetPersonByAllInfoList()
        {
            using (var httpClient = new HttpClient())
            {
                var personId = Guid.Parse("ce4bd23d-5f8a-426b-8aeb-74b7b800abcc");
               
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetPersonByAllInfoList/{personId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Assert.True(!string.IsNullOrEmpty(apiResponse));

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        var res = JsonConvert.DeserializeObject<StatusModel<ResAllPersonInfo>>(apiResponse);
                        Assert.True(res.Status.Status == Enums.StatusEnum.Successful); 
                    }
                }
            }
        }
        #endregion

        #region Kiþiden Ýletiþim Bilgileri Silmek
        [Fact]
        public async void DeletePersonInInfo1()
        {
            using (var httpClient = new HttpClient())
            {
                string contactInfoId = "88888888-8888-8888-8888-888888888888";
                using (var response = await httpClient.DeleteAsync($"{dbaApiUrl}/api/MainContact/Contact/DeleteInfoById/{contactInfoId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }
        [Fact]
        public async void DeletePersonByInfo()
        {
            using (var httpClient = new HttpClient())
            {
                string personId = "99999999-9999-9999-9999-999999999999";
                string contactInfoId = "3a506101-8724-4b70-ba1e-5166a4e01f3f";
                using (var response = await httpClient.DeleteAsync($"{dbaApiUrl}/api/MainContact/Contact/DeleteInfoToPerson/{personId}/{contactInfoId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful || res.Status.Status == Enums.StatusEnum.EmptyData);
                }
            }
        }
        #endregion

        #region Kiþi Silme Ýþlemleri
        #region Önce Eklenen Kiþi Silme Ýþlemi
        [Fact]
        public async void DeletePerson1()
        {
            using (var httpClient = new HttpClient())
            {
                string personId = "99999999-9999-9999-9999-999999999999";
                bool isHardDelete = true;
                using (var response = await httpClient.DeleteAsync($"{dbaApiUrl}/api/MainContact/Contact/DeletePerson/{personId}/{isHardDelete}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }
        #endregion
        #region Önce silinen veya olmayan veriyi silmeye çalýþmak
        [Fact]
        public async void DeletePerson2()
        {
            using (var httpClient = new HttpClient())
            {
                string personId = "99999999-9999-9999-9999-999999999999";
                bool isHardDelete = true;
                using (var response = await httpClient.DeleteAsync($"{dbaApiUrl}/api/MainContact/Contact/DeletePerson/{personId}/{isHardDelete}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);
                    //Silmek istediðiniz kiþi sistemde bulunamadý.
                    Assert.True(res.Status.Status == Enums.StatusEnum.Warning);
                }
            }
        }
        #endregion
        #endregion

        #region Rasgele 500 Veri Oluþturma
        [Fact]
        public async void CreateAllData()
        {
            /*
            Rasgele 500 Kiþi
            Her Kiþiye rasgele bir lokasyon (çokluda olabilir yapý uygun)
            Her Kiþiye rasgele 1 - 4 telefon numarasý
             */

            for (int i = 0; i < 500; i++)
            {
                using (var httpClient = new HttpClient())
                {
                    //DummyData ile Tam veri  Oluþturma
                    var dataJson = new ReqPersonContactModel()
                    {
                        PersonId = Guid.NewGuid(), //Her zaman yeni üretildiði için eklenmesi beklenir
                        Firstname = DummyData.RandomFirstName(),
                        Lastname = DummyData.RandomLastname(),
                        Company = DummyData.RandomCompanyName(),
                    };

                    var httpContent = new StringContent(dataJson.ToJson(), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreatePerson", httpContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<StatusModel<ResPersonContactModel>>(apiResponse);

                        if (res.Status.Status == Enums.StatusEnum.Successful)
                        {
                            using (var httpClient2 = new HttpClient())
                            {
                                var typ = Enums.ContactTypeEnum.Location;// DummyData.RandomContactType();
                                                                         //DummyData ile Tam veri  Oluþturma
                                var dataJson2 = new ReqPersonContactInfoModel()
                                {
                                    PersonId = dataJson.PersonId.Value,
                                    PersonContactId = Guid.NewGuid(),
                                    ContactType = typ,
                                    Info = DummyData.RandomContactTypeByInfo(typ)
                                };
                                var httpContent2 = new StringContent(dataJson2.ToJson(), Encoding.UTF8, "application/json");
                                using (var response2 = await httpClient2.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreateInfoToPerson", httpContent2))
                                {
                                    string apiResponse2 = await response2.Content.ReadAsStringAsync();
                                    var res2 = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse2);
                                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                                }
                            }

                            for (int ii = 0; ii < new Random().Next(1, 4); ii++)
                            {
                                using (var httpClient2 = new HttpClient())
                                {
                                    var typ = Enums.ContactTypeEnum.Phone;// DummyData.RandomContactType();
                                    //DummyData ile Tam veri  Oluþturma
                                    var dataJson2 = new ReqPersonContactInfoModel()
                                    {
                                        PersonId = dataJson.PersonId.Value,
                                        PersonContactId = Guid.NewGuid(),
                                        ContactType = typ,
                                        Info = DummyData.RandomContactTypeByInfo(typ)
                                    };
                                    var httpContent2 = new StringContent(dataJson2.ToJson(), Encoding.UTF8, "application/json");
                                    using (var response2 = await httpClient2.PostAsync($"{dbaApiUrl}/api/MainContact/Contact/CreateInfoToPerson", httpContent2))
                                    {
                                        string apiResponse2 = await response2.Content.ReadAsStringAsync();
                                        var res2 = JsonConvert.DeserializeObject<StatusModel<ResPersonContactInfoModel>>(apiResponse2);
                                        Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                                    }
                                }
                            }
                        }




                    }
                }
            }
        }
        #endregion

        #region Rapor Sorgulama Ýþlemleri
        [Fact]
        public async void GetReport()
        {
            using (var httpClient = new HttpClient())
            {
                var location = "Konya";
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetReportData/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<List<ResLocationReportModel>>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }
        [Fact]
        public async void GetAllReport()
        {
            using (var httpClient = new HttpClient())
            {
                var location = "ALL";
                using (var response = await httpClient.GetAsync($"{dbaApiUrl}/api/MainContact/Contact/GetReportData/{location}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<StatusModel<List<ResLocationReportModel>>>(apiResponse);
                    Assert.True(res.Status.Status == Enums.StatusEnum.Successful);
                }
            }
        }
        #endregion
    }
}