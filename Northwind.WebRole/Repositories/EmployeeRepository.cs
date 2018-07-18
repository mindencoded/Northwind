using System.Data.SqlClient;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(SqlConnection connection) : base(connection)
        {
        }

        public EmployeeRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public EmployeeRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public EmployeeRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public EmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public EmployeeRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}