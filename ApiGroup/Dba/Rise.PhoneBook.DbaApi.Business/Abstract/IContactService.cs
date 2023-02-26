using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.RequestModels;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.DbaApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Business.Abstract
{
    public interface IContactService
    {
        StatusModel<Contact> Get(Expression<Func<Contact, bool>> filter);
        StatusModel<IList<Contact>> GetList(Expression<Func<Contact, bool>> filter);
        StatusModel<Contact> Add(Contact entity);
        StatusModel<Contact> Update(Contact entity);
        StatusModel<Contact> Delete(Expression<Func<Contact, bool>> filter);
        StatusModel<ResPersonContactModel> AddCustom(ReqPersonContactModel person);

    }
}
