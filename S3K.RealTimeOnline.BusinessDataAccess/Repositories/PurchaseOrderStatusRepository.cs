using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.Repositories
{
    public class PurchaseOrderStatusRepository : Repository<PurchaseOrderStatus>, IPurchaseOrderStatusRepository
    {
        public PurchaseOrderStatusRepository(SqlConnection connection) : base(connection)
        {
        }

        public PurchaseOrderStatusRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public PurchaseOrderStatusRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public PurchaseOrderStatusRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) :
            base(connection, transaction, ignoreNulls)
        {
        }

        public PurchaseOrderStatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public PurchaseOrderStatusRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}