using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class SerializableDynamicObject : IDynamicMetaObjectProvider
    {
        [DataMember]
        private IDictionary<string, object> _dynamicProperties = new Dictionary<string, object>();

        #region IDynamicMetaObjectProvider implementation

        public DynamicMetaObject GetMetaObject(Expression expression)
        {
            return new SerializableDynamicMetaObject(expression, BindingRestrictions.GetInstanceRestriction(expression, this), this);
        }

        #endregion

        #region Helper methods for dynamic meta object support

        public void SetValue(string name, object value)
        {
            _dynamicProperties.Add(name, value);
        }

        private object GetValue(string name)
        {
            object value;
            _dynamicProperties.TryGetValue(name, out value);
            return value;
        }

        internal IEnumerable<string> GetDynamicMemberNames()
        {
            return _dynamicProperties.Keys;
        }

        public Dictionary<string, object> GetDictionary()
        {
            return GetDynamicMemberNames()
                .ToDictionary(x => x, x => GetValue(x));
        }

        #endregion
    }
}
