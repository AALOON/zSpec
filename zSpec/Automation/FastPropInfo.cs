using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using zSpec.Automation.Attributes;
// ReSharper disable StaticMemberInGenericType

namespace zSpec.Automation
{
    public class FastPropInfo<TSubject>
    {
        static FastPropInfo()
        {
            Attributes = FastTypeInfo<TSubject>.PublicProperties
                .ToDictionary(p => p.Name, p => p.GetCustomAttributes(inherit: true));

            PropertiesByColumnMap = FastTypeInfo<TSubject>.PublicProperties
                .GroupBy(p => GetName(p.Name))
                .ToDictionary(p => p.Key, p => p.ToArray());
        }

        public static Dictionary<string, object[]> Attributes { get; }

        public static Dictionary<string, PropertyInfo[]> PropertiesByColumnMap { get; }

        public static TAttribute FindAttribute<TAttribute>(string propName)
            where TAttribute : Attribute
        {
            return (TAttribute)Attributes[propName]
                .FirstOrDefault(p => p.GetType() == typeof(TAttribute));
        }

        /// <summary>
        /// Returns column name of attributes exists in property or default propName
        /// </summary>
        private static string GetName(string propName)
        {
            var attribute = Attributes[propName]
                .FirstOrDefault(p => p.GetType() == typeof(ColumnNameAttribute));
            if (attribute == null)
                return propName;

            var columnAttribute = (ColumnNameAttribute)attribute;

            return columnAttribute.Name;
        }
    }
}