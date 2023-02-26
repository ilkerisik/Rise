using Rise.PhoneBook.ApiCore.Core.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.DbaApi.TestProject
{
    public class DummyData
    {
        public static List<string> FirstnameList = new List<string>() { "HÜSEYİN", "MESUT", "LEYLA", "TUBA", "ALİ", "SALMAN", "ŞEYHMUS", "ZEHRA", "NURDAN", "CEBBAR", "ÖMER", "AHMET", "SİNAN", "ALPEREN", "SELAMİ", "ABDULAZİZ", "LEYLA", "FATMA", "MÜNEVER", "MÜRSELİN", "EMRULLAH", "EDA", "ÜMİT", "BÜNYAMİN", "RIDVAN", "ADEM", "ORHAN", "EMİNE", "KEMAL", "FATMA", "ALİ", "MEHMET", "MUSTAFA", "NEVZAT", "OSMAN", "ONUR", "MUSTAFA", "MEHTAP", "MURAT", "MUSA", "FİLİZ", "HALENUR", "KEZİBAN", "EFRUZ", "EMİN", "FATİH" };
        public static List<string> LastnameList = new List<string>() { "ŞEN", "KANDEMİR", "ÇEVİK", "ERKURAN", "TÜTEN", "ÖZTÜRK", "YÜZBAŞIOĞLU", "VURAL", "YÜCEL", "SÖNMEZ", "ERTEKİN", "DEDE", "UYANIK", "ASLAN", "AKBULUT", "ORHON", "UZ", "YAVUZ", "ERDEM", "KULAÇ", "KAYA", "SELVİ", "AKPINAR", "ABACIOĞLU", "ÇAY", "IŞIK", "ÖZER", "ÖZDEMİR", "ÖZTÜRK", "TAHTACI", "BÜYÜKCAM", "KULAKSIZ", "AKSEL", "EROĞLU", "KARAKUM", "DAL", "ÖCAL", "AYHAN", "YİĞİT", "YARBİL", "CANACANKATAN", "GÜMÜŞAY", "MURT", "HALHALLI", "ULUÖZ", "ŞEYHANLI", "ÇALIŞKANTÜRK", "YILMAZ", "SARAÇOĞLU", "SEZER", };
        public static List<string> CompanyNameList = new List<string>() { "Altın Mücevherat", "Ambalaj", "Ayakkabi", "Boya", "Demir Çelik", "Deri", "Doğal Taşlar", "Makine", "EvTekstili", "Gemi İnsa", "Gıda", "Halı", "Hazır Giyim", "Kimya", "Kozmetik", "Madencilik", "Mobilya", "Oto", "Tarım Aletleri", "Tekstil YanSanayi", "Tekstil ve Hammaddeleri", "Temizlik Maddeleri", "İlaçve Eczacılık", "İnşaat", "Eğitim", "Elektrik ve Elektronik", "Enerji", "Finans", };
        public static List<string> CoList = new List<string>() { "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkâri", "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce", };

        public static string RandomFirstName()
        {
            return FirstnameList.OrderBy(i => Guid.NewGuid()).FirstOrDefault();
        }

        public static string RandomLastname()
        {
            return LastnameList.OrderBy(i => Guid.NewGuid()).FirstOrDefault();
        }

        public static string RandomCompanyName()
        {
            return CompanyNameList.OrderBy(i => Guid.NewGuid()).FirstOrDefault();
        }
        public static string RandomContactTypeByInfo(Enums.ContactTypeEnum typeEnum)
        {
            string res = "";
            var rnd = new Random();
            switch (typeEnum)
            {
                case Enums.ContactTypeEnum.Phone:
                    res = $"0 {rnd.Next(543, 555)} {rnd.Next(100, 999)} {rnd.Next(10, 99)} {rnd.Next(10, 99)}";
                    break;
                case Enums.ContactTypeEnum.Mail:
                    res = $"{Guid.NewGuid().ToString().Split('-')[0]}@mail.com";
                    break;
                case Enums.ContactTypeEnum.Location:
                    res = CoList.OrderBy(i => Guid.NewGuid()).FirstOrDefault();
                    break;
                default:
                    break;
            }
            return res;
        }
    }
}
