using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Business.Abstract;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.DbaApi.Entities.Concrete;

namespace Rise.PhoneBook.DbaApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainContactController : ControllerBase
    {
        /*
         Beklenen işlevler:
                • Rehberde kişi oluşturma --> CreatePerson
                • Rehberde kişi kaldırma
                • Rehberdeki kişiye iletişim bilgisi ekleme
                • Rehberdeki kişiden iletişim bilgisi kaldırma
                • Rehberdeki kişilerin listelenmesi
                • Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi


                • Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor talebi
                • Sistemin oluşturduğu raporların listelenmesi
                • Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi
         */

        private readonly IContactService _contactService;

        public MainContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: api/MainContact/GetList
        [HttpGet("Contact/GetList")]
        public async Task<StatusModel<IList<Contact>>> GetList()
        {
            var listResult = new StatusModel<IList<Contact>>();
            await Task.Factory.StartNew(() =>
            {
                listResult = _contactService.GetList(i => true);
            });

            return listResult;
        }


        /// <summary>
        /// Kişi Ekleme Metodu
        /// </summary>
        /// <param name="person">Kişiye ait olan bilgiler model json olarak gönderilecektir</param>
        /// <returns></returns>
        [HttpPost("Contact/CreatePerson")]
        public async Task<StatusModel<ResPersonContactModel>> CreatePerson(ReqPersonContactModel person)
        {
            var result = new StatusModel<ResPersonContactModel>();
            await Task.Factory.StartNew(() =>
            {
                result = _contactService.AddCustom(person);
            });
            return result;
        }
    }
}
