using System;
using System.Collections.Generic;

namespace S3K.RealTimeOnline.CommonUtils
{
    internal class DecoratorTypeRegister
    {
        readonly Dictionary<Type, List<Type>> _decoratedTypes;

        public DecoratorTypeRegister()
        {
            _decoratedTypes = new Dictionary<Type, List<Type>>();
        }

        public void Register(
            Type typeToDecorate,
            Type decorateWith)
        {
            if (typeToDecorate == null)
                throw new ArgumentNullException("typeToDecorate");
            if (!typeToDecorate.IsInterface)
                throw new ArgumentException("TypeToDecorate must be an interface");
            if (decorateWith == null)
                throw new ArgumentNullException("decorateWith");

            List<Type> registeredDecoratorTypes;
            if (!_decoratedTypes.TryGetValue(typeToDecorate, out registeredDecoratorTypes))
            {
                registeredDecoratorTypes = new List<Type>();
                _decoratedTypes.Add(typeToDecorate, registeredDecoratorTypes);
            }
            registeredDecoratorTypes.Add(decorateWith);
        }

        public void Register<TTypeToDecorate, TDecorateWith>(
            TTypeToDecorate typeToDecorate,
            TDecorateWith decorateWith)
        {
            Register(typeof(TTypeToDecorate), typeof(TDecorateWith));
        }

        public IEnumerable<Type> GetDecoratorTypes(Type typeToDecorate)
        {
            if (typeToDecorate == null)
                throw new ArgumentNullException("typeToDecorate");
            if (!typeToDecorate.IsInterface)
                throw new ArgumentException("TypeToDecorate must be an interface");

            var result = new List<Type>();
            List<Type> registeredDecoratorTypes;
            if (_decoratedTypes.TryGetValue(typeToDecorate, out registeredDecoratorTypes))
                result.AddRange(registeredDecoratorTypes);
            if (typeToDecorate.IsGenericType)
                if (_decoratedTypes.TryGetValue(typeToDecorate.GetGenericTypeDefinition(),
                    out registeredDecoratorTypes))
                    result.AddRange(registeredDecoratorTypes);
            return result;
        }

        public IEnumerable<Type> GetDecoratorTypes<TTypeToDecorate>()
        {
            return GetDecoratorTypes(typeof(TTypeToDecorate));
        }

        public bool HasDecorators(Type decoratorType)
        {
            return _decoratedTypes.ContainsKey(decoratorType) ||
                   (decoratorType.IsGenericType &&
                    _decoratedTypes.ContainsKey(decoratorType.GetGenericTypeDefinition()));
        }

        internal void CopyFrom(DecoratorTypeRegister register)
        {
            foreach (var item in register._decoratedTypes)
            {
                List<Type> decoratorTypeList;
                if (!_decoratedTypes.TryGetValue(item.Key, out decoratorTypeList))
                {
                    decoratorTypeList = new List<Type>();
                    _decoratedTypes.Add(item.Key, decoratorTypeList);
                }
                decoratorTypeList.AddRange(item.Value);
            }
        }
    }
}