using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsumerSupport.Tests.Infrastructure
{
    public static class UniqueString
    {
        public static string Next => Guid.NewGuid().ToString();
    }

    public static class ReflectionHelper
    {
        public static T SetPropertyValue<T>(this T obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            property.SetValue(obj, value);
            return obj;
        }
    }
}
