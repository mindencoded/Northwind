using System.Data.SqlClient;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class UserTypeRepository : Repository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(SqlConnection connection) : base(connection)
        {
        }

        public UserTypeRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public UserTypeRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public UserTypeRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public UserTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserTypeRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}