using Rise.PhoneBook.ApiCore.Core.Entities;
using Rise.PhoneBook.DbaApi.Entities.ComplexTypes.ResponseModels;
using Rise.PhoneBook.DbaApi.Entities.Concrete;

namespace Rise.PhoneBook.DbaApi.DataAccess.Abstract
{
    public interface IContactInfoDal : IEntityRepository<ContactInfo>
    {
        List<ResLocationReportModel> GetReportData(string location);
    }
}
