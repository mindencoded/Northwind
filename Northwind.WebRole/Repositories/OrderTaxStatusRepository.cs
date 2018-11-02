using System.Data.SqlClient;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class OrderTaxStatusRepository : CommandRepository<OrderTaxStatus>, IOrderTaxStatusRepository
    {
        public OrderTaxStatusRepository(SqlConnection connection) : base(connection)
        {
        }

        public OrderTaxStatusRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public OrderTaxStatusRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public OrderTaxStatusRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public OrderTaxStatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public OrderTaxStatusRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}