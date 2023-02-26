using Rise.PhoneBook.ApiCore.Core;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
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

        public List<ResLocationReportModel> GetReportData(string location)
        {
            using (var context = new ContactContext())
            {
                var reportData = from contact in context.Contacts
                                 join infoLoc in context.ContactInfos
                                   on contact.Id equals infoLoc.ContactId
                                 join infoPho in context.ContactInfos
                                   on contact.Id equals infoPho.ContactId
                                 where (location == "" || infoLoc.Info == location) && infoLoc.ContactTypeId == 3 && infoPho.ContactTypeId != 3
                                 group new { x1 = infoLoc.Info, infoLocDef = infoLoc, infoPhoDeff = infoPho } by new
                                 {
                                     infoLoc.Info,
                                     infoLocVal = infoLoc.ContactTypeId,
                                     infoPhoVal = infoPho.ContactTypeId,
                                 } into grp
                                 select new ResLocationReportModel
                                 {
                                     Location = grp.Key.Info,
                                     PersonCount = grp.Select(x => x.infoLocDef.Id).Distinct().Count(),
                                     PhoneCount = grp.Select(x => x.infoPhoDeff.Id).Distinct().Count()
                                 };

                return reportData.ToList();
            }
        }

    }
}
