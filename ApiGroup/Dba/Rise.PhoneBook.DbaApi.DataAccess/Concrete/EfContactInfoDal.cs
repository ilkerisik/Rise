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
    public class EfContactInfoDal : EfEntityRepositoryBase<ContactInfo, ContactContext>, IContactInfoDal
    {
        protected override List<ContactInfo> GetList(Expression<Func<ContactInfo, bool>> filter, ContactContext context)
        {
            return filter == null
               ? context.ContactInfos.ToList()
               : context.ContactInfos.Where(filter).ToList();
        }
        protected override ContactInfo Get(Expression<Func<ContactInfo, bool>> filter, ContactContext context)
        {
            return context.ContactInfos.FirstOrDefault(filter);
        }
    }
}
