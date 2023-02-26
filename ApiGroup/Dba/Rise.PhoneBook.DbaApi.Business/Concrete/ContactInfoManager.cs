﻿using Rise.PhoneBook.ApiCore.Core.Custom;
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
    public class ContactInfoManager : IContactInfoService
    {
        private readonly IContactInfoDal _contactInfoDal;
        public ContactInfoManager(IContactInfoDal contactInfoDal)
        {
            _contactInfoDal = contactInfoDal;
        }
        public StatusModel<IList<ContactInfo>> GetList(Expression<Func<ContactInfo, bool>> filter)
        {
            var returnData = new StatusModel<IList<ContactInfo>>();
            try
            {
                returnData.Entity = _contactInfoDal.GetEntities(filter);
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
        public StatusModel<ContactInfo> Get(Expression<Func<ContactInfo, bool>> filter)
        {
            var returnData = new StatusModel<ContactInfo>();
            try
            {
                returnData.Entity = _contactInfoDal.Get(filter);
                if (returnData.Entity == null)
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
        public StatusModel<ContactInfo> Add(ContactInfo entity)
        {
            var returnData = new StatusModel<ContactInfo>();
            try
            {
                returnData.Entity = _contactInfoDal.Add(entity);
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
        public StatusModel<ContactInfo> Update(ContactInfo entity)
        {
            var returnData = new StatusModel<ContactInfo>();
            try
            {
                returnData.Entity = _contactInfoDal.Update(entity);
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
        public StatusModel<ContactInfo> Delete(Expression<Func<ContactInfo, bool>> filter)
        {
            var returnData = new StatusModel<ContactInfo>();
            try
            {
                var entity = _contactInfoDal.Get(filter);
                if (entity == null)
                {
                    returnData.Status.Message = "Silinecek veri bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
                else
                {
                    _contactInfoDal.Delete(entity);
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
        public StatusModel<ResPersonContactInfoModel> AddInfoToPerson(Contact contact, ReqPersonContactInfoModel info)
        {
            var returnData = new StatusModel<ResPersonContactInfoModel>();
            try
            {
                ContactInfo? returnDataEntity = null;
                var data = new ContactInfo()
                {
                    ContactId = contact.Id,
                    ContactTypeId = (int)info.ContactType,
                    Info = info.Info,
                    CreatedOn = DateTime.UtcNow,

                };
                //Eklenmek istenen İletişim için Id gönderilmezse üret ?
                if (!info.PersonContactId.HasValue || info.PersonContactId == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                    returnDataEntity = _contactInfoDal.Add(data);
                    returnData.Status.Message = "Kişiye iletişim bilgisi eklendi.";
                }
                else
                {
                    //Kişi Id gönderildi ise sistemde var mı kontrol et ve ekle
                    var existData = _contactInfoDal.Get(i => i.Id == info.PersonContactId);
                    if (existData == null)
                    {
                        data.Id = info.PersonContactId.Value;
                        returnDataEntity = _contactInfoDal.Add(data);
                        returnData.Status.Message = "Kişiye iletişim bilgisi eklendi.";
                    }
                    else
                    {
                        returnData.Status.Message = "Eklemek istediğiniz iletişim id sistemde tanımlı!";
                    }
                }
                if (returnDataEntity == null)
                {
                    returnData.Status.Status = Enums.StatusEnum.Warning;
                    if (string.IsNullOrEmpty(returnData.Status.Message))
                        returnData.Status.Message = "Kişiye iletişim bilgisi eklenemedi.";
                }
                else
                {
                    returnData.Entity = new ResPersonContactInfoModel()
                    {
                        PersonContactId = returnDataEntity.Id,
                        PersonId = returnDataEntity.ContactId,
                        ContactType = (Enums.ContactTypeEnum)returnDataEntity.ContactTypeId,
                        Info = returnDataEntity.Info
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
        public StatusModel<ResPersonContactInfoModel> DeleteCustom(Expression<Func<ContactInfo, bool>> filter)
        {
            var returnData = new StatusModel<ResPersonContactInfoModel>();
            try
            {
                var entity = _contactInfoDal.Get(filter);
                if (entity == null)
                {
                    returnData.Status.Message = "Silinecek veri bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
                else
                {
                    returnData.Entity = new ResPersonContactInfoModel()
                    {
                        PersonId = entity.ContactId,
                        PersonContactId = entity.Id,
                        ContactType = (Enums.ContactTypeEnum)entity.ContactTypeId,
                        Info = entity.Info
                    };
                    _contactInfoDal.Delete(entity);
                    returnData.Status.Message = "Silme İşlemi Başarılı";
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
