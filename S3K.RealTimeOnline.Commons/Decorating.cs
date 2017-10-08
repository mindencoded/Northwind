using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace S3K.RealTimeOnline.Commons
{
    public class Decorating : UnityContainerExtension
    {
        DecoratorTypeRegister _register;

        public Decorating()
        { }

        internal Decorating(IUnityContainer container, DecoratorTypeRegister register)
        {
            container.AddExtension(this);
            _register.CopyFrom(register);
        }

        protected override void Initialize()
        {
            _register = new DecoratorTypeRegister();
            Context.ChildContainerCreated += Context_ChildContainerCreated;
            Context.Strategies.Add(
                new DecoratingBuildStrategy(_register),
                UnityBuildStage.PreCreation
            );
        }

        void Context_ChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            var decorating = new Decorating(e.ChildContainer, _register);
        }

        public IUnityContainer Decorate(Type typeToDecorate, Type decorateWith)
        {
            _register.Register(typeToDecorate, decorateWith);
            return Container;
        }

        public IUnityContainer Decorate<TTypeToDecorate, TDecorateWith>()
        {
            Decorate(typeof(TTypeToDecorate), typeof(TDecorateWith));
            return Container;
        }
    }
}
