using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Rise.PhoneBook.ReportApi.Entities.ComplexTypes.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ReportApi.Business.Concrete
{
    public class ExcelFactoryService : IExcelFactoryService
    {
        public StatusModel<string> AllModelToExcelFileSave(string requestId, List<ResLocationReportModel.Entity> resReportDetailModel)
        {
            var returnData = new StatusModel<string>();
            try
            {
                var data = new List<object[]>();
                data.AddRange(resReportDetailModel.Select(i => new object[] { i.location, i.personCount, i.phoneCount }).ToArray());
                data.Insert(0, new object[] { "Location", "Person Count", "Phone Count" });
                using var wb = new XLWorkbook();
                var ws = wb.AddWorksheet();
                ws.Cell("A1").InsertData(data);
                ws.Cell("A1").Style.Font.FontSize = 13;
                ws.Cell("B1").Style.Font.FontSize = 13;
                ws.Cell("C1").Style.Font.FontSize = 13;

                string file = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "/Content/";
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                    file += requestId + ".xlsx";
                }

                wb.SaveAs(file);
                returnData.Entity = "/Content/" + requestId + ".xlsx";
                returnData.Status.Message = "İşlem Başarılı";
                returnData.Status.Status = Enums.StatusEnum.Successful;
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
    }
}
