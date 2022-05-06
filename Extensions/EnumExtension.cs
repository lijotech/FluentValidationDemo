using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApiProject.Extensions
{
    public static class EnumExtension
    {
        public static List<KeyValuePair<string, string>> GetEnumValuesAndDescriptions<T>()
        {
            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");
            List<KeyValuePair<string, string>> enumValList = new List<KeyValuePair<string, string>>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                enumValList.Add(new KeyValuePair<string, string>(e.ToString(), (attributes.Length > 0) ? attributes[0].Description : e.ToString()));
            }
            return enumValList;
        }
    }
}
