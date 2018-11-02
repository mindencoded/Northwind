using System.Data.SqlClient;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class InvoiceRepository : CommandRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(SqlConnection connection) : base(connection)
        {
        }

        public InvoiceRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public InvoiceRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public InvoiceRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public InvoiceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public InvoiceRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}