using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;

namespace Rise.PhoneBook.ReportApi.Business.Abstract
{
    public interface IExcelFactoryService
    {
        StatusModel<string> AllModelToExcelFileSave(string requestId, List<ResLocationReportModel.Entity> resReportDetailModel);
    }
}
