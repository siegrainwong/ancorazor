using System;
using System.Linq;

namespace Siegrain.Common
{
    public static class AttributeExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
                return att;
            return null;
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            if (type.GetAttribute<TAttribute>() is TAttribute att)
                return valueSelector(att);

            return default;
        }
    }
}
