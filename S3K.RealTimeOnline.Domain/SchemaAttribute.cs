using System;

namespace S3K.RealTimeOnline.Domain
{
    public class SchemaAttribute : Attribute
    {
        public readonly string Name;

        public SchemaAttribute(string name)
        {
            Name = name;
        }
    }
}