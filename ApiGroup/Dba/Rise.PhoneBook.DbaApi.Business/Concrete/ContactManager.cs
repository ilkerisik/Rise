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
    public class ContactManager : IContactService
    {
        public StatusModel<Contact> Add(Contact entity)
        {
            throw new NotImplementedException();
        }

        public StatusModel<Contact> Delete(Expression<Func<Contact, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public StatusModel<Contact> Get(Expression<Func<Contact, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public StatusModel<IList<Contact>> GetList(Expression<Func<Contact, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public StatusModel<Contact> Update(Contact entity)
        {
            throw new NotImplementedException();
        }
    }
}
