using Microsoft.WindowsAzure.ServiceRuntime;

namespace Northwind.WebRole
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // Para obtener información sobre cómo administrar los cambios de configuración
            // consulte el tema de MSDN en https://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}