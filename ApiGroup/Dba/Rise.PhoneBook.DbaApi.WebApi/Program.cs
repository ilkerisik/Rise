using Microsoft.OpenApi.Models;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.DbaApi.Business.Abstract;
using Rise.PhoneBook.DbaApi.Business.Concrete;
using Rise.PhoneBook.DbaApi.DataAccess.Abstract;
using Rise.PhoneBook.DbaApi.DataAccess.Concrete;
using System.Text.Json.Serialization;

namespace Rise.PhoneBook.DbaApi.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddTransient<IContactService, ContactManager>();
            builder.Services.AddTransient<IContactDal, EfContactDal>();

            builder.Services.AddTransient<IContactInfoService, ContactInfoManager>();
            builder.Services.AddTransient<IContactInfoDal, EfContactInfoDal>();


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                            c =>
                            {
                                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rise Contact Api", Version = "v1" });
                                c.SchemaFilter<Tools.EnumSchemaFilter>();
                                c.UseAllOfToExtendReferenceSchemas();
                                c.CustomOperationIds(e => $"{ExtensionMethods.RootControllerNameEdit(e.ActionDescriptor.AttributeRouteInfo.Template)}{e.ActionDescriptor.RouteValues["action"]}");
                            });

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();

            app.Run();
        }
    }
}