using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rise.PhoneBook.ApiCore.Core.Custom
{
    public static class ExtensionMethods
    {
        public static string RootControllerNameEdit(string s)
        {
            var path = s.Split('/');
            if (path.Length >= 2)
            {
                return path[1];
            }
            return "";
        }

        public static string ToJson(this object target)
        {
            return JsonConvert.SerializeObject(target);
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        public static string ToByteArrayUtf8String(this byte[] obj)
        {
            return Encoding.UTF8.GetString(obj);
        }

        public static List<string> EnumListOf<T>(bool isDescription = false)
        {
            Type type = typeof(T);
            if (!type.IsEnum) return null;
            if (isDescription == false)
                return System.Enum.GetValues(type).Cast<System.Enum>().Select(x => x.ToString()).ToList();
            else
                return System.Enum.GetValues(type).Cast<System.Enum>().Select(x => x.GetDescription()).ToList();
        }
        public static string GetDescription(this System.Enum value)
        {
            try
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                if (fieldInfo == null) return value.ToString();

                var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
                return attribute.Description;
            }
            catch (Exception)
            {
            }

            return "";
        }

        public static byte[] ToByteArray(this object obj)
        {
            return ((byte[])(obj));
        }
        public static T FromJson<T>(this string target)
        {
            if (string.IsNullOrEmpty(target))
                target = "";
            return JsonConvert.DeserializeObject<T>(target);
        }

        public static T JsonToClass<T>(this byte[] classType)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(classType));
        }
        public static byte[] ToJsonByteArray(this object target)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(target));
        }

    }
}
