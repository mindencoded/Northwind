using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.Repositories
{
    public class OrderDetailStatusRepository : Repository<OrderDetailStatus>, IOrderDetailStatusRepository
    {
        public OrderDetailStatusRepository(SqlConnection connection) : base(connection)
        {
        }

        public OrderDetailStatusRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public OrderDetailStatusRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public OrderDetailStatusRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection, transaction, ignoreNulls)
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
