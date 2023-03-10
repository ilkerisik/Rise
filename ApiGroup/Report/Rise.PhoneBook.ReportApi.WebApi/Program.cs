using Microsoft.OpenApi.Models;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Rise.PhoneBook.ReportApi.Business.Concrete;
using Rise.PhoneBook.ReportApi.DataAccess.Abstract;
using Rise.PhoneBook.ReportApi.DataAccess.Concrete;
using Rise.PhoneBook.ReportApi.WebApi.BgService;
using System.Text.Json.Serialization;

namespace Rise.PhoneBook.ReportApi.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IQueueProcessorService, QueueProcessorService>();
            builder.Services.AddTransient<IReportQueueProcessDal, EfReportQueueProcessDal>();
            builder.Services.AddTransient<IReportQueueProcessService, ReportQueueProcessManager>();
            builder.Services.AddTransient<IExcelFactoryService, ExcelFactoryService>();

            //Rapor Servisi içerisindeki Background servisi ile RabbitMq koordinasyonu sağlamak
            builder.Services.AddHostedService<MqHostedService>();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                            c =>
                            {
                                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rise Report Api", Version = "v1" });
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