using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.Business.Abstract
{
    public interface IContactInfoService
    {
        StatusModel<ContactInfo> Get(Expression<Func<ContactInfo, bool>> filter);
        StatusModel<IList<ContactInfo>> GetList(Expression<Func<ContactInfo, bool>> filter);
        StatusModel<ContactInfo> Add(ContactInfo entity);
        StatusModel<ContactInfo> Update(ContactInfo entity);
        StatusModel<ContactInfo> Delete(Expression<Func<ContactInfo, bool>> filter);
    }
}
