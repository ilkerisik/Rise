using Newtonsoft.Json;
using Rise.PhoneBook.ApiCore.Core.Custom;
using Rise.PhoneBook.ApiCore.Core.Models;
using Rise.PhoneBook.ReportApi.Business.Abstract;
using Rise.PhoneBook.ReportApi.DataAccess.Abstract;
using Rise.PhoneBook.ReportApi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Rise.PhoneBook.ApiCore.Core.Custom.Enums;

namespace Rise.PhoneBook.ReportApi.Business.Concrete
{
    public class ReportQueueProcessManager : IReportQueueProcessService
    {
        private readonly IReportQueueProcessDal _reportQueueProcessDal;
        public ReportQueueProcessManager(IReportQueueProcessDal reportQueueProcess)
        {
            _reportQueueProcessDal = reportQueueProcess;
        }
        public StatusModel<IList<ReportQueueProcess>> GetList(Expression<Func<ReportQueueProcess, bool>> filter)
        {
            var returnData = new StatusModel<IList<ReportQueueProcess>>();
            try
            {
                returnData.Entity = _reportQueueProcessDal.GetEntities(filter);
                if (returnData.Entity.Count == 0)
                {
                    returnData.Status.Message = "Veri Bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.EmptyData;
                }
                else
                {
                    returnData.Status.Message = "İşlem Başarılı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<ReportQueueProcess> Get(Expression<Func<ReportQueueProcess, bool>> filter)
        {
            var returnData = new StatusModel<ReportQueueProcess>();
            try
            {
                returnData.Entity = _reportQueueProcessDal.Get(filter);

                if (returnData.Entity == null)
                {
                    returnData.Status.Message = "Kişi Verisi Bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.EmptyData;
                }
                else
                {
                    returnData.Status.Message = "İşlem Başarılı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<ReportQueueProcess> Add(ReportQueueProcess entity)
        {
            var returnData = new StatusModel<ReportQueueProcess>();
            try
            {
                returnData.Entity = _reportQueueProcessDal.Add(entity);

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
        public StatusModel<ReportQueueProcess> Update(ReportQueueProcess entity)
        {
            var returnData = new StatusModel<ReportQueueProcess>();
            try
            {
                returnData.Entity = _reportQueueProcessDal.Update(entity);

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
        public StatusModel<ReportQueueProcess> Delete(Expression<Func<ReportQueueProcess, bool>> filter)
        {
            var returnData = new StatusModel<ReportQueueProcess>();
            try
            {
                var entity = _reportQueueProcessDal.Get(filter);
                if (entity == null)
                {
                    returnData.Status.Message = "Silinecek veri bulunamadı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
                else
                {
                    _reportQueueProcessDal.Delete(entity);
                    returnData.Status.Message = "İşlem Başarılı";
                    returnData.Status.Status = Enums.StatusEnum.Successful;
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public StatusModel<ReportQueueProcess> AddOrUpdate(ReportQueueProcess entity)
        {
            var returnData = new StatusModel<ReportQueueProcess>();
            try
            {
                var data = Get(i => i.Id == entity.Id);
                returnData.Status = data.Status;
                if (data.Status.Status == StatusEnum.Successful)
                {
                    if (entity.QueueStatus.IsNotNullOrEmpty() && data.Entity.QueueStatus != entity.QueueStatus)
                        data.Entity.QueueStatus = entity.QueueStatus;

                    if (data.Entity.Datas != entity.Datas)
                        data.Entity.Datas = entity.Datas;

                    if (entity.Queueprocess.IsNotNullOrEmpty() && data.Entity.Queueprocess != entity.Queueprocess)
                        data.Entity.Queueprocess = entity.Queueprocess;

                    if (entity.LastQueueName.IsNotNullOrEmpty() && data.Entity.LastQueueName != entity.LastQueueName)
                        data.Entity.LastQueueName = entity.LastQueueName;

                    if (entity.Filename.IsNotNullOrEmpty() && data.Entity.Filename != entity.Filename)
                        data.Entity.Filename = entity.Filename;

                    if (entity.ChangedOn.HasValue && data.Entity.ChangedOn != entity.ChangedOn)
                        data.Entity.ChangedOn = entity.ChangedOn;

                    if (entity.State.HasValue && data.Entity.State != entity.State)
                        data.Entity.State = entity.State;

                    return Update(data.Entity);
                }
                else if (data.Status.Status == StatusEnum.EmptyData)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                    return Add(entity);
                }
            }
            catch (Exception ex)
            {
                returnData.Status.Message = "Hata Oluştu";
                returnData.Status.Exception = ex.Message;
                returnData.Status.Status = Enums.StatusEnum.Error;
            }
            return returnData;
        }
        public bool QueueProcessAdd(string requestId, byte[] sendData, List<MqProcessPriority> header_QueueProcess, Enums.StatusEnum status, string lastProcessName, string fileName)
        {
            var lastProcess = header_QueueProcess.LastOrDefault(i => i.IsRunned);
            var dataTemp = new ReportQueueProcess()
            {
                Id = Guid.Parse(requestId),
                LastQueueName = lastProcessName,
                Queueprocess = header_QueueProcess.ToJson(),
                QueueStatus = lastProcess == null ? "" : lastProcess.Result,
                ChangedOn = DateTime.UtcNow,
                Datas = sendData == null ? null : sendData.ToByteArrayUtf8String(),
                State = (short)status,
                Filename = fileName,
            };
            var data = AddOrUpdate(dataTemp);

            if (data.Status.Status == Enums.StatusEnum.Successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public StatusModel<byte[]> QReport(byte[] data, ref List<MqProcessPriority> mqProcesses)
        {
            StatusModel<byte[]> res = new StatusModel<byte[]>();
            var resLocal = mqProcesses.FirstOrDefault(i => i.MqKey == Enums.QueueProcess.QReport.ToString() && i.Status != StatusEnum.Successful);
            resLocal.IsRunned = true;

            #region İlk adımda yapılacak işlemler bu kısımda yapılacaktır.
            //var dataRes = data.JsonToClass<MainQueueRequest>();
            //res.Entity = dataRes.ToJsonByteArray(); 
            #endregion

            res.Status.Status = StatusEnum.Successful;
            res.Status.Message = "Rapor Sorgulanıyor";
            res.Entity = data;

            resLocal.Status = res.Status.Status;
            resLocal.Result = res.Status.Status == StatusEnum.Successful ? "Rapor Sorgulanıyor" : res.Status.Message;
            resLocal.IsBreak = res.Status.Status == StatusEnum.Successful ? false : true;
            return res;
        }
        /// <summary>
        /// Api sorgulaması ve dosya oluşturma işlemleri
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mqProcesses"></param>
        /// <returns></returns>
        public StatusModel<byte[]> QFile(byte[] data, ref List<MqProcessPriority> mqProcesses)
        {
            StatusModel<byte[]> res = new StatusModel<byte[]>();
            var dataRes = data.JsonToClass<MainQueueRequest>();

            var resLocal = mqProcesses.FirstOrDefault(i => i.MqKey == Enums.QueueProcess.QReportProcess.ToString() && i.Status != StatusEnum.Successful);
            resLocal.IsRunned = true;

            #region DbaApi Sorgulaması Yapılacak
            if (true)
            {

                //Kuyruklama sıralı olarak senkron çalışıyor mu diye beklemeye alındı
                Thread.Sleep(new Random().Next(1000, 10000));
                dataRes.FilePath = "/test.txt";
                dataRes.Message = "Dosya Hazırlandı";
                res.Status.Status = StatusEnum.Successful;
                res.Status.Message = dataRes.FilePath;
            }
            else
            {
                dataRes.Message = "Dosya Hazırlanamadı!";
                res.Status.Status = StatusEnum.Warning;
                res.Status.Message = "";
            }
            #endregion

            res.Entity = dataRes.ToJsonByteArray();

            resLocal.Status = res.Status.Status;
            resLocal.Result = resLocal.Status == StatusEnum.Successful ? dataRes.Message : res.Status.Message;
            resLocal.IsBreak = res.Status.Status == StatusEnum.Successful ? false : true;
            return res;
        }

        /// <summary>
        /// Kuyruktaki işlemler bitince bu alana gelmesi planlandı.
        /// İşlemlerin bittiği bu kısımda kullanıcıya bildirilecek
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mqProcesses"></param>
        /// <returns></returns>
        public StatusModel<string> QReportLast(byte[] data, ref List<MqProcessPriority> mqProcesses)
        {
            StatusModel<string> res = new StatusModel<string>();
            var resLocal = mqProcesses.FirstOrDefault(i => i.MqKey == Enums.QueueProcess.QReportLastControl.ToString() && i.Status != StatusEnum.Successful);
            resLocal.IsRunned = true;

            res.Entity = "İşlem Başarılı";
            res.Status.Status = Enums.StatusEnum.Successful;
            res.Status.Message = "İşlem Başarılı";

            resLocal.Status = res.Status.Status;
            resLocal.Result = resLocal.Status == StatusEnum.Successful ? "Tamamlandı" : res.Status.Message;
            resLocal.IsBreak = res.Status.Status == StatusEnum.Successful ? false : true;

            return res;
        }
    }
}
