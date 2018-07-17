using System;

namespace Northwind.WebRole.Domain
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