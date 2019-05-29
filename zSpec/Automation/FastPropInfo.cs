using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using zSpec.Automation.Attributes;

// ReSharper disable StaticMemberInGenericType

namespace zSpec.Automation
{
    public static class FastPropInfo<TSubject>
    {
        private static readonly FastPropInfo PropInfo;

        static FastPropInfo()
        {
            PropInfo = FastPropInfo.GetInstance(typeof(TSubject));
        }

        public static Dictionary<string, object[]> Attributes => PropInfo.Attributes;

        public static Dictionary<string, PropertyInfo[]> PropertiesByColumnMap => PropInfo.PropertiesByColumnMap;

        public static TAttribute FindAttribute<TAttribute>(string propName)
            where TAttribute : Attribute
        {
            return PropInfo.FindAttribute<TAttribute>(propName);
        }
    }

    public class FastPropInfo
    {
        private static readonly ConcurrentDictionary<Type, FastPropInfo> Cache
            = new ConcurrentDictionary<Type, FastPropInfo>();

        public FastPropInfo(Type type)
        {
            const bool inherit = true;
            var typeInfo = FastTypeInfo.GetInstance(type);
            Attributes = typeInfo.PublicProperties
                .ToDictionary(p => p.Name, p => p.GetCustomAttributes(inherit));

            PropertiesByColumnMap = typeInfo.PublicProperties
                .GroupBy(p => GetName(p.Name))
                .ToDictionary(p => p.Key, p => p.ToArray());
        }

        public Dictionary<string, object[]> Attributes { get; }

        public Dictionary<string, PropertyInfo[]> PropertiesByColumnMap { get; }

        public TAttribute FindAttribute<TAttribute>(string propName)
            where TAttribute : Attribute
        {
            return (TAttribute) Attributes[propName]
                .FirstOrDefault(p => p.GetType() == typeof(TAttribute));
        }

        public static FastPropInfo GetInstance(Type type)
        {
            return Cache.GetOrAdd(type, new FastPropInfo(type));
        }

        /// <summary>
        /// Returns column name of attributes exists in property or default propName
        /// </summary>
        private string GetName(string propName)
        {
            var attribute = Attributes[propName]
                .FirstOrDefault(p => p.GetType() == typeof(ColumnNameAttribute));
            if (attribute == null)
            {
                return propName;
            }

            var columnAttribute = (ColumnNameAttribute) attribute;

            return columnAttribute.Name;
        }
    }
}