using Rise.PhoneBook.ApiCore.Core;
using Rise.PhoneBook.ReportApi.DataAccess.Abstract;
using Rise.PhoneBook.ReportApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rise.PhoneBook.ReportApi.DataAccess.Concrete
{
    public class EfReportQueueProcessDal : EfEntityRepositoryBase<ReportQueueProcess, ReportContext>, IReportQueueProcessDal
    {
        protected override List<ReportQueueProcess> GetList(Expression<Func<ReportQueueProcess, bool>> filter, ReportContext context)
        {
            return filter == null
               ? context.ReportQueueProcesses.ToList()
               : context.ReportQueueProcesses.Where(filter).ToList();
        }
        protected override ReportQueueProcess Get(Expression<Func<ReportQueueProcess, bool>> filter, ReportContext context)
        {
            return context.ReportQueueProcesses.FirstOrDefault(filter);
        }
    }
}
