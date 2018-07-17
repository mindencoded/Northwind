using System.Data.SqlClient;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class InventoryTransactionTypeRepository : Repository<InventoryTransactionType>,
        IInventoryTransactionTypeRepository
    {
        public InventoryTransactionTypeRepository(SqlConnection connection) : base(connection)
        {
        }

        public InventoryTransactionTypeRepository(SqlConnection connection, bool ignoreNulls) : base(connection,
            ignoreNulls)
        {
        }

        public InventoryTransactionTypeRepository(SqlConnection connection, SqlTransaction transaction) : base(
            connection, transaction)
        {
        }

        public InventoryTransactionTypeRepository(SqlConnection connection, SqlTransaction transaction,
            bool ignoreNulls) : base(connection, transaction, ignoreNulls)
        {
        }

        public InventoryTransactionTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public InventoryTransactionTypeRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork,
            ignoreNulls)
        {
        }
    }
}