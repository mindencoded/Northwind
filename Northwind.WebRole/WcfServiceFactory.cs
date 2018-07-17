using Unity;
using Unity.Wcf;

namespace Northwind.WebRole
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            ConfigContainer configContainer = new ConfigContainer();
            configContainer.Instance(container);
        }
    }
}