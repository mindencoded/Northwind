using System.Data.SqlClient;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class InventoryTransactionRepository : Repository<InventoryTransaction>, IInventoryTransactionRepository
    {
        public InventoryTransactionRepository(SqlConnection connection) : base(connection)
        {
        }

        public InventoryTransactionRepository(SqlConnection connection, bool ignoreNulls) : base(connection,
            ignoreNulls)
        {
        }

        public InventoryTransactionRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public InventoryTransactionRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) :
            base(connection, transaction, ignoreNulls)
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