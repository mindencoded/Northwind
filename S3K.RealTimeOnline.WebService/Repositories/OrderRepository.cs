using System.Data.SqlClient;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(SqlConnection connection) : base(connection)
        {
        }

        public OrderRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public OrderRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public OrderRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public OrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public OrderRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}