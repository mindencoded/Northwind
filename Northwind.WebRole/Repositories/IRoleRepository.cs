using Northwind.WebRole.Domain.Security;

namespace Northwind.WebRole.Repositories
{
    public interface IRoleRepository : ICommandRepository<Role>
    {
    }
}