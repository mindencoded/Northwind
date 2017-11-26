using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.Repositories
{
    public class PurchaseOrderDetailRepository : Repository<PurchaseOrderDetail>, IPurchaseOrderDetailRepository
    {
        public PurchaseOrderDetailRepository(SqlConnection connection) : base(connection)
        {
        }

        public PurchaseOrderDetailRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public PurchaseOrderDetailRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public PurchaseOrderDetailRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) :
            base(connection, transaction, ignoreNulls)
        {
        }

        public PurchaseOrderDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public PurchaseOrderDetailRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}