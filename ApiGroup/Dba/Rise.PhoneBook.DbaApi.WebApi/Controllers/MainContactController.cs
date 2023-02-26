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
                • Rehberde kişi kaldırma --> DeletePerson
                • Rehberdeki kişiye iletişim bilgisi ekleme --> CreateInfoToPerson
                • Rehberdeki kişiden iletişim bilgisi kaldırma --> DeleteInfoById
                • Rehberdeki kişilerin listelenmesi --> GetAllPersonList
                • Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi --> GetPersonByAllInfoList

                • Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor talebi
                • Sistemin oluşturduğu raporların listelenmesi
                • Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi
         */

        private readonly IContactService _contactService;
        private readonly IContactInfoService _contactInfoService;
        public MainContactController(IContactService contactService, IContactInfoService contactInfoService)
        {
            _contactService = contactService;
            _contactInfoService = contactInfoService;
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
        /// Rehberde kişi oluşturma
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

        /// <summary>
        /// Kişi silme ve ilişkisel verileri silme metodu
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="isHardDelete"></param>
        /// <returns></returns>
        [HttpDelete("Contact/DeletePerson/{personId}/{isHardDelete}")]
        public async Task<StatusModel<ResPersonContactModel>> DeletePerson(Guid personId, bool isHardDelete)
        {
            var deleteResult = new StatusModel<ResPersonContactModel>();
            await Task.Factory.StartNew(() =>
            {
                deleteResult = _contactService.DeleteCustom(i => i.Id == personId, isHardDelete);
            });

            return deleteResult;
        }

        /// <summary>
        /// Rehberdeki kişiye iletişim bilgisi ekleme
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("Contact/CreateInfoToPerson")]
        public async Task<StatusModel<ResPersonContactInfoModel>> CreateInfoToPerson(ReqPersonContactInfoModel info)
        {
            var result = new StatusModel<ResPersonContactInfoModel>();

            var checkResult = new StatusModel<Contact>();

            await Task.Factory.StartNew(() =>
            {
                checkResult = _contactService.Get(i => i.Id == info.PersonId);
            });

            if (checkResult.Status.Status == Enums.StatusEnum.Successful)
            {
                await Task.Factory.StartNew(() =>
                {
                    result = _contactInfoService.AddInfoToPerson(checkResult.Entity, info);
                });
            }
            else
            {
                result.Status = checkResult.Status;
            }
            return result;
        }

        /// <summary>
        /// Rehberdeki kişiden iletişim bilgisi kaldırma
        /// </summary>
        /// <param name="contactInfoId"></param>
        /// <returns></returns>
        [HttpDelete("Contact/DeleteInfoById/{contactInfoId}")]
        public async Task<StatusModel<ResPersonContactInfoModel>> DeleteInfoById(Guid contactInfoId)
        {
            var deleteResult = new StatusModel<ResPersonContactInfoModel>();
            await Task.Factory.StartNew(() =>
            {
                deleteResult = _contactInfoService.DeleteCustom(i => i.Id == contactInfoId);
            });
            return deleteResult;
        }

        [HttpDelete("Contact/DeleteInfoToPerson/{personId}/{contactInfoId}")]
        public async Task<StatusModel<ResPersonContactInfoModel>> DeleteInfoToPerson(Guid personId, Guid contactInfoId)
        {
            var deleteResult = new StatusModel<ResPersonContactInfoModel>();

            var checkResult = new StatusModel<Contact>();

            await Task.Factory.StartNew(() =>
            {
                checkResult = _contactService.Get(i => i.Id == personId);
            });


            if (checkResult.Status.Status == Enums.StatusEnum.Successful)
            {
                await Task.Factory.StartNew(() =>
                {
                    deleteResult = _contactInfoService.DeleteCustom(i => i.Id == contactInfoId);
                });
            }
            else
            {
                deleteResult.Status = checkResult.Status;
            }
            return deleteResult;
        }

        /// <summary>
        /// Rehberdeki kişilerin listelenmesi
        /// </summary>
        /// <returns></returns>
        [HttpGet("Contact/GetAllPersonList")]
        public async Task<StatusModel<IList<ResPersonContactModel>>> GetAllPersonList()
        {
            var result = new StatusModel<IList<ResPersonContactModel>>();
            await Task.Factory.StartNew(() =>
            {
                result = _contactService.GetListCustom(i => true);
            });
            return result;
        }

        /// <summary>
        /// Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("Contact/GetPersonByAllInfoList/{personId}")]
        public async Task<StatusModel<ResAllPersonInfo>> GetPersonByAllInfoList(Guid personId)
        {
            var result = new StatusModel<ResAllPersonInfo>();

            var checkResult = new StatusModel<Contact>();

            await Task.Factory.StartNew(() =>
            {
                checkResult = _contactService.Get(i => i.Id == personId);
            });

            if (checkResult.Status.Status == Enums.StatusEnum.Successful)
            {
                await Task.Factory.StartNew(() =>
                {
                    result = _contactService.GetCustomPersonByAllInfo(checkResult.Entity, personId);
                });
            }
            else
            {
                result.Status = checkResult.Status;
            }

            return result;
        }


    }
}