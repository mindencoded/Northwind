using System.Data.SqlClient;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SqlConnection connection) : base(connection)
        {
        }

        public CustomerRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public CustomerRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public CustomerRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public CustomerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public CustomerRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}