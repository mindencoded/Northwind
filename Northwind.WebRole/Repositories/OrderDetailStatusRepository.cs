using System.Data.SqlClient;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class OrderDetailStatusRepository : Repository<OrderDetailStatus>, IOrderDetailStatusRepository
    {
        public OrderDetailStatusRepository(SqlConnection connection) : base(connection)
        {
        }

        public OrderDetailStatusRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public OrderDetailStatusRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public OrderDetailStatusRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) :
            base(connection, transaction, ignoreNulls)
        {
        }

        public OrderDetailStatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public OrderDetailStatusRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}