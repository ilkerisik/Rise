using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Business.Abstract;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.DbaApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Business.Concrete
{
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;
        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }
        public StatusModel<IList<Contact>> GetList(Expression<Func<Contact, bool>> filter)
        {
            var returnData = new StatusModel<IList<Contact>>();
            try
            {
                returnData.Entity = _contactDal.GetEntities(filter);
                if (returnData.Entity == null || returnData.Entity.Count == 0)
                {
                    returnData.Status.Message = "Veri Bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.EmptyData;
                }
                else
                {
                    returnData.Status.Message = "İşlem Başarılı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<Contact> Get(Expression<Func<Contact, bool>> filter)
        {
            var returnData = new StatusModel<Contact>();
            try
            {
                returnData.Entity = _contactDal.Get(filter);

                if (returnData.Entity == null)
                {
                    returnData.Status.Message = "Kişi Verisi Bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.EmptyData;
                }
                else
                {
                    returnData.Status.Message = "İşlem Başarılı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<Contact> Add(Contact entity)
        {
            var returnData = new StatusModel<Contact>();
            try
            {
                returnData.Entity = _contactDal.Add(entity);

                returnData.Status.Message = "İşlem Başarılı";
                returnData.Status.Status = Enums.StatusEnum.Successful;
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<Contact> Update(Contact entity)
        {
            var returnData = new StatusModel<Contact>();
            try
            {
                returnData.Entity = _contactDal.Update(entity);

                returnData.Status.Message = "İşlem Başarılı";
                returnData.Status.Status = Enums.StatusEnum.Successful;
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<Contact> Delete(Expression<Func<Contact, bool>> filter)
        {
            var returnData = new StatusModel<Contact>();
            try
            {
                var entity = _contactDal.Get(filter);
                if (entity == null)
                {
                    returnData.Status.Message = "Silinecek veri bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
                else
                {
                    _contactDal.Delete(entity);
                    returnData.Status.Message = "İşlem Başarılı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<ResPersonContactModel> AddCustom(ReqPersonContactModel person)
        {
            var returnData = new StatusModel<ResPersonContactModel>();
            try
            {
                Contact? returnDataEntity = null;
                var g = Guid.NewGuid();
                var data = new Contact()
                {
                    Firstname = person.Firstname,
                    Lastname = person.Lastname,
                    Company = person.Company,
                    CreatedOn = DateTime.UtcNow
                };

                //Eklenmek istenen Kişi için Id gönderilmezse üret ?
                if (!person.PersonId.HasValue || person.PersonId == Guid.Empty)
                {
                    data.Id = g;
                    returnDataEntity = _contactDal.Add(data);
                    returnData.Status.Message = "Kişi eklendi.";
                }
                else
                {
                    //Kişi Id gönderildi ise sistemde var mı kontrol et ve ekle
                    var existData = _contactDal.Get(i => i.Id == person.PersonId);
                    if (existData == null)
                    {
                        data.Id = person.PersonId.Value;
                        returnDataEntity = _contactDal.Add(data);
                        returnData.Status.Message = "Kişi eklendi.";
                    }
                    else
                    {
                        returnData.Status.Message = "Eklemek istediğiniz id sistemde tanımlı!";
                    }
                }
                if (returnDataEntity == null)
                {
                    returnData.Status.Status = Enums.StatusEnum.Warning;
                    if (string.IsNullOrEmpty(returnData.Status.Message))
                        returnData.Status.Message = "Kişi eklenemedi.";
                }
                else
                {
                    returnData.Entity = new ResPersonContactModel()
                    {
                        PersonId = returnDataEntity.Id,
                        Company = returnDataEntity.Company,
                        Firstname = returnDataEntity.Firstname,
                        Lastname = returnDataEntity.Lastname,
                        CreatedOn = returnDataEntity.CreatedOn,
                    };
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }

    }
}
