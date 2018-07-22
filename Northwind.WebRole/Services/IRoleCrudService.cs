using System.ServiceModel;
using Northwind.Shared;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IRoleCrudService : ICrudService<RoleDto>
    {
    }
}