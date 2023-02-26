
# Rise PhoneBook Test Project

Bu proje .Net Core, RabbitMq, Web Api, Postgresql kullanarak basit bir Telefon Defteri oluşturmak amacı ile tasarlanmıştır.



## Proje Hakkında Özet

Proje mikro servis kullanılarak kişilerin ve bilgilerinin sisteme eklenmesi ve kuyruk yapısı içerisinde sıralı şekilde darboğaz olmadan rapor almak için tasarlanmıştır.

Proje İçeriği;
```
Nice
-- ApiGroup
---- ApiCore
------ Rise.PhoneBook.ApiCore.Core

-- ApiGroup
---- Dba
------ Rise.PhoneBook.DbaApi.WebApi
------ Rise.PhoneBook.DbaApi.Entities
------ Rise.PhoneBook.DbaApi.DataAccess
------ Rise.PhoneBook.DbaApi.Business

-- ApiGroup
---- Report
------ Rise.PhoneBook.ReportApi.WebApi
------ Rise.PhoneBook.ReportApi.Entities
------ Rise.PhoneBook.ReportApi.DataAccess
------ Rise.PhoneBook.ReportApi.Business

-- Test
---- Rise.PhoneBook.DbaApi.TestProject
---- Rise.PhoneBook.ReportApi.TestProject
```


İşleyiş olarak;
* Kişi ve ek bilgileri DbaApi ye gönderilir, Api veriyi Postgresql veritabanına aktarır. DbaApi ile aşağıdaki metotlar yardımı ile Ekleme, Silme, Bilgi Ekleme, Bilgi Silme işlemleri yapılabilir ve mevcut veriler listelenebilir. Bu işlemleri yaparken Postman, WebClient veya Swagger kullanılabilir. Varsayılan olarak   http://localhost:5206 adresinden ulaşabilirsiniz.

* DbaApi veritabanına ulaşabilen bir apidir ve veri ekleme silme, güncelleme ve listeleme özelliklerine sahiptir. Raporlamalar içinde ReportApi nin bağlanması için bir uç noktası vardır.

* Eklenen bilgilerin raporları alınabilmesi için ReportApi metoları kullanılacaktır. Bu api içerisinde rapor talepleri, kuyruk yönetimi, rapor durum kontrolleri ve rapor excel dosyalarını indirme metotları bulunmaktadır.

* ReportApi (http://localhost:5236) dışarıdan gelen lokasyon bazlı rapor talebini alır ve RabbitMq (http://localhost:15672/) ya gönderir. ReportApi içerisinde bulunan BackgroundService ile kuyruk içerindeki tetik alınır ve işlenmeye başlar. Kuyruk 3 aşamalı (QReport,QReportProcess,QReportLastControl) tasarlandı. Hepsi ayrı dinleyicidir ve işlemleri aralarında taşıyarak kendi içlerinde de kuruk verilerini koordine etmektedirler. RabbitMq alınan talebi uygulama kapanıp açılsada yerine getirmek için sıradan devam edecektir.

* Kuyruk **QReport** ile başlayacak ve kontroller yapılacaktır. **QReportProcess** ile DbaApi ye ilgili Lokasyon parametresi ile istek gönderecek ve gelen cevaba göre Excel dosyasını oluşturup ilgili klasöre kayıt edecek ve kuruktaki durumu değiştirerek **QReportLastControl** durumuna geçecek ve son adımda veritabanındaki ilgili alanı "Tamamlandı" olarak düzenleyecektir.

* Kuyruk ve raporlama işlemleri sonlandıktan sonra veya işlem sırasında ReportApi den gönderilen talep sonucunda gelen **RequestId** ile istekler yapılarak ilgili işlemin sonucu öğrenebilir ve duruma göre mesajda gelen dosyayı yine ReportApi metotları ile indirebilirsiniz.



  
## Kullanılan Teknolojiler ve Nuget Paketleri 


.Net Core, Postgresql, Swagger, RabbitMq, Katmanlı Mimari, Mikro Servis Mimarisi, Web Api, Entity Framework


Proje içerisindeki kullanılan paketler

```bash
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Tools
Microsoft.Extensions.Configuration
Microsoft.Extensions.Configuration.Binder
Microsoft.Extensions.Configuration.EnvironmentVariables
Microsoft.Extensions.Configuration.Json
Microsoft.Extensions.Configuration.Abstractions
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.EntityFrameworkCore.Design
Swashbuckle.AspNetCore
ClosedXML
Newtonsoft.Json
RabbitMQ.Client
xunit
```

  
## Metotlar

- Rehberde kişi oluşturma --> **CreatePerson**

  ```http
  POST /api/MainContact/Contact/CreatePerson
```
- Rehberde kişi kaldırma --> **DeletePerson**

  ```http
  DELETE /api/MainContact/Contact/DeletePerson/{personId}/{isHardDelete}
```


- Rehberdeki kişiye iletişim bilgisi ekleme --> **CreateInfoToPerson**

```http
  POST /api/MainContact/Contact/CreateInfoToPerson
```

- Rehberdeki kişiden iletişim bilgisi kaldırma --> **DeleteInfoById**

```http
delete /api/MainContact/Contact/DeleteInfoById/{contactInfoId}
```

```http
delete /api/MainContact/Contact/DeleteInfoToPerson/{personId}/{contactInfoId}
```

- Rehberdeki kişilerin listelenmesi --> **GetAllPersonList**

```http
get /api/MainContact/Contact/GetAllPersonList
```

- Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi --> **GetPersonByAllInfoList**

```http
get /api/MainContact/Contact/GetPersonByAllInfoList/{personId}
```

- Rehberdeki bilgilere göre Lokasyon bazlı rapor gösterilmesi
location şehir bilgisi veya ALL olarak gönderilecektir.
--> **GetPersonByAllInfoList**
```http
get /api/MainContact/Contact/GetReportData/{location}
```

- Rapor istekleri için Lokasyon değerinin Kuruğa işlenmek üzere gönderilmesi --> **QueueSend**

```http
get /api/QueueProcess/QueueSend/{location}
```

- Rapor isteklerinin kuyruktaki durumunun sorgulanması --> **GetReportStatus**
```http
get /api/Report/GetReportStatus/{requestId}
```

- Tüm bitmiş ve devam eden kuyruktaki verilerin durumları --> **GetAllReportStatus**
```http
get /api/Report/GetAllReportStatus
```

- Hazırlanan raporların indirilmesi --> **DownloadReport**
```http
get /api/Report/DownloadReport/{requestId}
```
## İşlemler

Bu projeyi çalıştırmak için aşağıdaki ortam değişkenlerini eklemelisiniz

Key : `RABBITMQ_NODENAME` Value : `COMPUTERNAME`

  
Migration için aşağıdaki kodlar çalıştırılmalıdır 

`add-migration Initial -Context "ContactContext" -StartupProject "Rise.PhoneBook.DbaApi.WebApi" -Project "Rise.PhoneBook.DbaApi.DataAccess"`

`add-migration Initial -Context "ReportContext" -StartupProject "Rise.PhoneBook.ReportApi.WebApi" -Project "Rise.PhoneBook.ReportApi.DataAccess"`

`Update-Database`

