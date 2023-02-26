using Newtonsoft.Json;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.DataAccess.Concrete;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.DbaApi.Entities.Concrete;
using System.Text;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels;

namespace Rise.PhoneBook.DbaApi.TestProject
{
    public class MainUnitTest
    {
        [Fact]
        public void MainTest()
        {

        }
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
    }
}