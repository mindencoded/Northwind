using System;
using System.Collections.Generic;
using S3K.RealTimeOnline.Commons;

namespace S3K.RealTimeOnline.Service
{
    public class Container : IContainer
    {
        public delegate object Creator(Container container);

        private readonly Dictionary<Type, Creator> _typeToCreator = new Dictionary<Type, Creator>();

        public Dictionary<string, object> Configuration { get; } = new Dictionary<string, object>();

        public T Resolve<T>()
        {
            var type = typeof(T);
            return (T) _typeToCreator[type](this);
        }

        public object Resolve(Type type)
        {
            return _typeToCreator[type](this);
        }

        public void Register<T>(Creator creator)
        {
            _typeToCreator.Add(typeof(T), creator);
        }

        public T GetConfiguration<T>(string name)
        {
            return (T) Configuration[name];
        }
    }
}