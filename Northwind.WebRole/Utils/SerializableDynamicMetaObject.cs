using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace Northwind.WebRole.Utils
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
            Expression self = Expression;
            ConstantExpression keyExpr = Expression.Constant(binder.Name);
            MethodInfo getMethod = _type.GetMethod("getValue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getMethod == null) return null;
            MethodCallExpression target = Expression.Call(Expression.Convert(self, _type), getMethod, keyExpr);
            return new DynamicMetaObject(target, BindingRestrictions.GetTypeRestriction(self, _type));
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            Expression self = Expression;
            ConstantExpression keyExpr = Expression.Constant(binder.Name);
            UnaryExpression valueExpression = Expression.Convert(value.Expression, typeof(object));
            MethodInfo setMethod = _type.GetMethod("setValue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (setMethod == null) return null;
            Expression target = Expression.Call(Expression.Convert(self, _type),
                setMethod,
                keyExpr,
                valueExpression);
            return new DynamicMetaObject(target, BindingRestrictions.GetTypeRestriction(self, _type));
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            SerializableDynamicObject serializableDynamicObject = (SerializableDynamicObject) Value;
            return serializableDynamicObject.GetDynamicMemberNames();
        }
    }
}