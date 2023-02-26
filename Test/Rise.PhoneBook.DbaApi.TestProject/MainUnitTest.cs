using Newtonsoft.Json;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.DataAccess.Concrete;
using Rise.PhoneBook.DbaApi.Entities.Concrete;

namespace Rise.PhoneBook.DbaApi.TestProject
{
    public class MainUnitTest
    {
        [Fact]
        public void MainTest()
        {

        }
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

    }
}