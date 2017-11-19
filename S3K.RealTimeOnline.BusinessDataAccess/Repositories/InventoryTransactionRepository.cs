using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.Repositories
{
    public class InventoryTransactionRepository : Repository<InventoryTransaction>, IInventoryTransactionRepository
    {
        public InventoryTransactionRepository(SqlConnection connection) : base(connection)
        {
        }

        public InventoryTransactionRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public InventoryTransactionRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public InventoryTransactionRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection, transaction, ignoreNulls)
        {
        }

        public InventoryTransactionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public InventoryTransactionRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}
