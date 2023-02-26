using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Business.Abstract;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
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

    }
}
