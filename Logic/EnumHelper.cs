using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CvGenerator.Logic
{
    public static class EnumHelper<T>
    {
        public static IEnumerable<T> GetValues()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IEnumerable<string> GetNames()
        {
            return Enum.GetNames(typeof(T));
        }

        public static IEnumerable<string> GetDisplayNames()
        {
            return GetNames().Select(n => GetDisplayName(n)).ToList();
        }

        public static string GetDisplayName(string value)
        {
            var members = typeof(T).GetMember(value);
            if (members == null || members.Length == 0)
                return value;
            Attribute attr = members[0].GetCustomAttribute<DisplayAttribute>();
            if (attr != null)
                return ((DisplayAttribute)attr).Name;
            return value;
        }
    }
}
