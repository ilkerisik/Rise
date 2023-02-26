using Rise.PhoneBook.DbaApi.Business.Abstract;
using Rise.PhoneBook.DbaApi.Business.Concrete;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
using Rise.PhoneBook.DbaApi.DataAccess.Concrete;

namespace Rise.PhoneBook.DbaApi.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IContactService, ContactManager>();
            builder.Services.AddTransient<IContactDal, EfContactDal>();

            builder.Services.AddTransient<IContactInfoService, ContactInfoManager>();
            builder.Services.AddTransient<IContactInfoDal, EfContactInfoDal>();


            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}