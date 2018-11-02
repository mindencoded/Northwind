using System.Data.SqlClient;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class RoleGroupRepository : CommandRepository<RoleGroup>, IRoleGroupRepository
    {
        public RoleGroupRepository(SqlConnection connection) : base(connection)
        {
        }

        public RoleGroupRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public RoleGroupRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public RoleGroupRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public RoleGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public RoleGroupRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}