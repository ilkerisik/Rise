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
        #region Do�rudan Veritaban� Eri�im ��lemleri
        [Fact]
        public void DbContactAddTest()
        {
            using (var c = new ContactContext())
            {
                var data = c.Contacts.Add(new Entities.Concrete.Contact()
                {
                    Id = Guid.NewGuid(),
                    Firstname = "�lker",
                    Lastname = "I�IK",
                    Company = "I�IK A.�",
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

        #region Api Eri�im Testi
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
        #region Ki�i Ekleme ��lemleri
        #region Unique Veriler ile Tam ��erik G�nderme Testi
        [Fact]
        public async void CreatePerson1()
        {
            using (var httpClient = new HttpClient())
            {
                //DummyData ile Tam veri  Olu�turma
                var dataJson = new ReqPersonContactModel()
                {
                    PersonId = Guid.NewGuid(), //Her zaman yeni �retildi�i i�in eklenmesi beklenir
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
        #region �lk veri olmayan ikinci veri �nce eklenen veri olacak
        [Fact]
        public async void CreatePerson2()
        {
            //�lk ��lem Ba�ar�l� olarak beklenmekte
            using (var httpClient = new HttpClient())
            {
                //DummyData ile Company bo� veri  Olu�turma
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

            //�kinci veri Daha �nce Eklendi
            using (var httpClient = new HttpClient())
            {
                //DummyData ile Tam veri  Olu�turma
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
                    //Eklemek istedi�iniz id sistemde tan�ml�!
                    Assert.True(res.Status.Status == Enums.StatusEnum.Warning);
                }
            }
        }
        #endregion 
        #endregion


        #region Ki�i ye Bilgi Ekleme ��lemleri
        #region Unique Veriler ile Tam ��erik G�nderme Testi
        [Fact]
        public async void CreatePersonInfoByLocation1()
        {
            using (var httpClient = new HttpClient())
            {
                #region Bilgi olarak Rasgele Lokasyon Ekle
                var typ = Enums.ContactTypeEnum.Location;
                //DummyData ile Tam veri  Olu�turma
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
                    //DummyData ile Tam veri  Olu�turma
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
        #region �lk veri olmayan ikinci veri �nce eklenen veri olacak
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
                                //DummyData ile Tam veri  Olu�turma
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

        #region T�m Ki�ileri Listele
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

        #region Ki�iler ve T�m Bilgilerini Listele
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

        #region Ki�iden �leti�im Bilgileri Silmek
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

        #region Ki�i Silme ��lemleri
        #region �nce Eklenen Ki�i Silme ��lemi
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
        #region �nce silinen veya olmayan veriyi silmeye �al��mak
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
                    //Silmek istedi�iniz ki�i sistemde bulunamad�.
                    Assert.True(res.Status.Status == Enums.StatusEnum.Warning);
                }
            }
        }
        #endregion
        #endregion
    }
}