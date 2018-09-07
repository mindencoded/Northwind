using System.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Northwind.WebRole
{
    public class WebRole : RoleEntryPoint
    {
        private static readonly TraceSource Trace = new TraceSource("Northwind.WebRole");
        public override bool OnStart()
        {
            // Para obtener información sobre cómo administrar los cambios de configuración
            // consulte el tema de MSDN en https://go.microsoft.com/fwlink/?LinkId=166357.
            Trace.TraceInformation("WebRole is running.");
            return base.OnStart();
        }
    }
}