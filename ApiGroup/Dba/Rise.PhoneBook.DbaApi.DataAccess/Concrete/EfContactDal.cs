using Rise.PhoneBook.ApiCore.Core;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
using Rise.PhoneBook.DbaApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.DataAccess.Concrete
{
    public class EfContactDal : EfEntityRepositoryBase<Contact, ContactContext>, IContactDal
    {
        protected override List<Contact> GetList(Expression<Func<Contact, bool>> filter, ContactContext context)
        {
            return filter == null
               ? context.Contacts.ToList()
               : context.Contacts.Where(filter).ToList();
        }
        protected override Contact Get(Expression<Func<Contact, bool>> filter, ContactContext context)
        {
            return context.Contacts.FirstOrDefault(filter);
        }
    }
}
