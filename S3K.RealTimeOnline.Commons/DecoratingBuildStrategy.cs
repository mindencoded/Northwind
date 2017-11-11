using System;
using System.Collections.Generic;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Interception.Utilities;
using Unity.Resolution;

namespace S3K.RealTimeOnline.Commons
{
    public class DecoratingBuildStrategy : BuilderStrategy
    {
        readonly DecoratorTypeRegister _register;

        internal DecoratingBuildStrategy(DecoratorTypeRegister register)
        {
            _register = register;
        }


        public override void PostBuildUp(IBuilderContext context)
        {
            base.PostBuildUp(context);
            Type typeRequested = context.OriginalBuildKey.Type;
            if (!typeRequested.IsInterface)
                return;

            if (!_register.HasDecorators(typeRequested))
                return;

            var typeStack = new Stack<Type>(_register.GetDecoratorTypes(typeRequested));

            typeStack.ForEach(decoratorType =>
            {
                DependencyOverride dependencyOverride = new DependencyOverride(
                    typeRequested,
                    context.Existing
                );

                Type actualTypeToBuild = decoratorType;
                if (actualTypeToBuild.IsGenericTypeDefinition)
                {
                    Type[] genericArgumentTypes = context.OriginalBuildKey.Type.GetGenericArguments();
                    actualTypeToBuild = actualTypeToBuild.MakeGenericType(genericArgumentTypes);
                }

                context.AddResolverOverrides(dependencyOverride);
                context.Existing = context.NewBuildUp(new NamedTypeBuildKey(actualTypeToBuild));
            });
        }
    }
}