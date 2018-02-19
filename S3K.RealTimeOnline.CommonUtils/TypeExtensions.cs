using System;

namespace S3K.RealTimeOnline.CommonUtils
{
    public static class TypeExtensions
    {
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }
    }
}