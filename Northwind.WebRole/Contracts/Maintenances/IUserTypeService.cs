using System.ServiceModel;
using Northwind.WebRole.Dtos;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IUserTypeService : IMaintenanceService<UserTypeDto>
    {
    }
}