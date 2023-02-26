using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
