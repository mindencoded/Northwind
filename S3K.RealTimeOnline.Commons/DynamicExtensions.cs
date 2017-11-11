using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace S3K.RealTimeOnline.Commons
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value, bool ignoreNulls = true)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            {
                if (ignoreNulls && property.GetValue(value) == null) continue;
                expando.Add(property.Name, property.GetValue(value));
            }

            return expando as ExpandoObject;
        }
    }
}