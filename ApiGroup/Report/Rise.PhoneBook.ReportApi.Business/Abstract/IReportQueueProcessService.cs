using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ApiCore.Core.Models;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.ReportApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Rise.PhoneBook.ApiCore.Core.Custom.Enums;

namespace Rise.PhoneBook.ReportApi.Business.Abstract
{
    public interface IReportQueueProcessService
    {
        StatusModel<ReportQueueProcess> Get(Expression<Func<ReportQueueProcess, bool>> filter);
        StatusModel<IList<ReportQueueProcess>> GetList(Expression<Func<ReportQueueProcess, bool>> filter);
        StatusModel<ReportQueueProcess> Add(ReportQueueProcess entity);
        StatusModel<ReportQueueProcess> Update(ReportQueueProcess entity);
        StatusModel<ReportQueueProcess> Delete(Expression<Func<ReportQueueProcess, bool>> filter);
        StatusModel<ReportQueueProcess> AddOrUpdate(ReportQueueProcess entity);
        bool QueueProcessAdd(string requestId, byte[] sendData, List<MqProcessPriority> header_QueueProcess, Enums.StatusEnum status, string lastProcessName, string fileName);
        StatusModel<byte[]> QReport(byte[] data, ref List<MqProcessPriority> mqProcesses);
        StatusModel<byte[]> QFile(byte[] data, ref List<MqProcessPriority> mqProcesses);
        StatusModel<string> QReportLast(byte[] data, ref List<MqProcessPriority> mqProcesses);
        Task<ResLocationReportModel> GetReport(string url, string location);
        StatusModel<ResReportDetailModel> GetReportStatus(string url,Guid requestId);
        StatusModel<List<ResReportDetailModel>> GetAllReportStatus(string url);

    }
}
