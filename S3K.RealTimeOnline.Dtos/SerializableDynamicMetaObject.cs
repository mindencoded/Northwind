using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace S3K.RealTimeOnline.Dtos
{
    public class SerializableDynamicMetaObject : DynamicMetaObject
    {
        readonly Type _type;

        public SerializableDynamicMetaObject(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
            _type = value.GetType();
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var self = Expression;
           var dynObj = (SerializableDynamicObject) Value;
            var keyExpr = Expression.Constant(binder.Name);
            var getMethod = _type.GetMethod("getValue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getMethod == null) return null;
            var target = Expression.Call(Expression.Convert(self, _type), getMethod, keyExpr);
            return new DynamicMetaObject(target, BindingRestrictions.GetTypeRestriction(self, _type));
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var self = Expression;
            var keyExpr = Expression.Constant(binder.Name);
            var valueExpression = Expression.Convert(value.Expression, typeof(object));
            var setMethod = _type.GetMethod("setValue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (setMethod == null) return null;
            var target = Expression.Call(Expression.Convert(self, _type),
                setMethod,
                keyExpr,
                valueExpression);
            return new DynamicMetaObject(target, BindingRestrictions.GetTypeRestriction(self, _type));
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var serializableDynamicObject = (SerializableDynamicObject)Value;
            return serializableDynamicObject.GetDynamicMemberNames();
        }
    }
}
