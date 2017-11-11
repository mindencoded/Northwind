using System;

namespace S3K.RealTimeOnline.GenericDomain
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