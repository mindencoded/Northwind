using System;

namespace S3K.RealTimeOnline.Domain.Entities
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