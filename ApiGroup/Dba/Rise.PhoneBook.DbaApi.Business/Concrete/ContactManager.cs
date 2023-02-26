using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Business.Abstract;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
using Rise.PhoneBook.DbaApi.DataAccess.Concrete;
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
        private readonly IContactInfoDal _contactInfoDal;

        public ContactManager(IContactDal contactDal, IContactInfoDal contactInfoDal)
        {
            _contactDal = contactDal;
            _contactInfoDal = contactInfoDal;   
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
        public StatusModel<ResPersonContactModel> DeleteCustom(Expression<Func<Contact, bool>> filter, bool isHardDelete)
        {
            var returnData = new StatusModel<ResPersonContactModel>();
            try
            {
                var entity = _contactDal.Get(filter);
                if (entity == null)
                {
                    returnData.Status.Message = "Silmek istediğiniz kişi sistemde bulunamadı.";
                    returnData.Status.Status = Enums.StatusEnum.Warning;
                }
                else
                {

                    if (entity.ContactInfos.Count > 0)
                    {
                        if (isHardDelete)
                        {
                            foreach (var item in entity.ContactInfos)
                            {
                                _contactInfoDal.Delete(item);
                            }
                            _contactDal.Delete(entity);
                            returnData.Status.Message = "Kişiye ait tüm bilgiler Silindi.";
                            returnData.Status.Status = Enums.StatusEnum.Successful;
                        }
                        else
                        {
                            returnData.Status.Message = "Silme işlemi ilişkisel veriler olduğu için yapılmadı. İletişim bilgilerini silin yada Tüm veriyi kaldır olarak işaretleyin.";
                            returnData.Status.Status = Enums.StatusEnum.Warning;
                        }

                    }
                    else
                    {
                        _contactDal.Delete(entity);
                        returnData.Status.Message = "Kişi Silme Başarılı";
                        returnData.Status.Status = Enums.StatusEnum.Successful;
                    }
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
        public StatusModel<IList<ResPersonContactModel>> GetListCustom(Expression<Func<Contact, bool>> filter)
        {
            var returnData = new StatusModel<IList<ResPersonContactModel>>();

            try
            {
                var returnDataEntity = _contactDal.GetEntities(filter);
                if (returnDataEntity == null || returnDataEntity.Count == 0)
                {
                    returnData.Status.Message = "Veri Bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.EmptyData;
                }
                else
                {
                    returnData.Entity = returnDataEntity.Select(i => new ResPersonContactModel()
                    {
                        PersonId = i.Id,
                        Firstname = i.Firstname,
                        Lastname = i.Lastname,
                        Company = i.Company,
                        CreatedOn = i.CreatedOn
                    }).ToList();

                    returnData.Status.Message = $"İşlem Başarılı. {returnData.Entity.Count} adet veri listelendi.";
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
        public StatusModel<ResAllPersonInfo> GetCustomPersonByAllInfo(Contact entity, Guid personId)
        {
            var returnData = new StatusModel<ResAllPersonInfo>();
            try
            {
                returnData.Entity = new ResAllPersonInfo()
                {
                    PersonId = entity.Id,
                    Firstname = entity.Firstname,
                    Lastname = entity.Lastname,
                    Company = entity.Company,
                    CreatedOn = entity.CreatedOn,
                };
                var returnDataEntity = _contactInfoDal.GetEntities(i => i.ContactId == personId);
                returnData.Entity.ContactInfoList = returnDataEntity.Select(i => new ResPersonContactInfoModel()
                {
                    PersonId = i.ContactId,
                    PersonContactId = i.ContactId,
                    ContactType = (Enums.ContactTypeEnum)i.ContactTypeId,
                    Info = i.Info
                }).ToList();
                if (returnDataEntity.Count == 0)
                    returnData.Status.Message = $"İşlem Başarılı ancak {returnDataEntity.Count} adet veri listelendi.";
                else
                    returnData.Status.Message = $"İşlem Başarılı. Kişiye ait {returnDataEntity.Count} adet veri listelendi.";

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
    }
}
