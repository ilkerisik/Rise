using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Business.Abstract;
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
        public StatusModel<ContactInfo> Add(ContactInfo entity)
        {
            throw new NotImplementedException();
        }

        public StatusModel<ContactInfo> Delete(Expression<Func<ContactInfo, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public StatusModel<ContactInfo> Get(Expression<Func<ContactInfo, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public StatusModel<IList<ContactInfo>> GetList(Expression<Func<ContactInfo, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public StatusModel<ContactInfo> Update(ContactInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}
