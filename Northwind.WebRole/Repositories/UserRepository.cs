using System.Data.SqlClient;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class UserRepository : CommandRepository<User>, IUserRepository
    {
        public UserRepository(SqlConnection connection) : base(connection)
        {
        }

        public UserRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public UserRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public UserRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection,
            transaction, ignoreNulls)
        {
        }

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}