using System.Data.SqlClient;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.Repositories
{
    public class OrderTaxStatusRepository : Repository<OrderTaxStatus>, IOrderTaxStatusRepository
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