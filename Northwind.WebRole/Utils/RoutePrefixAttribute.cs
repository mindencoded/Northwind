using System;

namespace Northwind.WebRole.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutePrefixAttribute : Attribute
    {
        private string _name;

        public RoutePrefixAttribute(string name)
        {
            _name = name;
        }
        
        public virtual string Name
        {
            get {return _name;}
        }
    }
}