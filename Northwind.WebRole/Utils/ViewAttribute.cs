using System;

namespace Northwind.WebRole.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute
    {
        public ViewAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}