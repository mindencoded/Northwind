using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.Repositories
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(SqlConnection connection) : base(connection)
        {
        }

        public PurchaseOrderRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public PurchaseOrderRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public PurchaseOrderRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public PurchaseOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public PurchaseOrderRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}