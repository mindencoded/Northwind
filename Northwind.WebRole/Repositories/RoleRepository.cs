using System.Data.SqlClient;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(SqlConnection connection) : base(connection)
        {
        }

        public RoleRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public RoleRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public RoleRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection,
            transaction, ignoreNulls)
        {
        }

        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public RoleRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}