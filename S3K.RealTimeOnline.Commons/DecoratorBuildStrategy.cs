using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace S3K.RealTimeOnline.Commons
{
    public class DecoratorBuildStrategy : BuilderStrategy
    {
      
        private readonly Dictionary<Type, List<Type>> _typeStacks;

        public DecoratorBuildStrategy(Dictionary<Type, List<Type>> typeStacks)
        {
            _typeStacks = typeStacks;
        }


        public override void PreBuildUp(IBuilderContext context)
        {
            var key = context.OriginalBuildKey;

            if (!(key.Type.IsInterface && _typeStacks.ContainsKey(key.Type)))
            {
                return;
            }

            if (null != context.GetOverriddenResolver(key.Type))
            {
                return;
            }

            Stack<Type> stack = new Stack<Type>(
                _typeStacks[key.Type]
            );

            object value = null;
            stack.ForEach(
                t =>
                {
                    value = context.NewBuildUp(
                        new NamedTypeBuildKey(t, key.Name)
                    );
                    var overrides = new DependencyOverride(
                        key.Type,
                        value
                    );
                    context.AddResolverOverrides(overrides);
                }
            );

            context.Existing = value;
            context.BuildComplete = true;
        }
    }
}
