using System;
using System.Linq;

namespace Northwind.Shared
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
            Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            TAttribute att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att != null)
                return valueSelector(att);
            return default(TValue);
        }
    }
}