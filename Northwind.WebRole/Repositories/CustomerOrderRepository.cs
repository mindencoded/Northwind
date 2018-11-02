
using System.Data.SqlClient;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class CustomerOrderRepository : QueryRepository<CustomerOrder>, ICustomerOrderRepository
    {
        public CustomerOrderRepository(SqlConnection connection) : base(connection)
        {
        }

        public CustomerOrderRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public CustomerOrderRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public CustomerOrderRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public CustomerOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public CustomerOrderRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}