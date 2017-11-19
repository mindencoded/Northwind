using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(SqlConnection connection) : base(connection)
        {
        }

        public OrderDetailRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public OrderDetailRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public OrderDetailRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection, transaction, ignoreNulls)
        {
        }

        public OrderDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public OrderDetailRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}
