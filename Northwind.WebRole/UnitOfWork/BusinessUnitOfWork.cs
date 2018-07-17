using System.Data.SqlClient;

namespace Northwind.WebRole.UnitOfWork
{
    public class BusinessUnitOfWork : UnitOfWork, IBusinessUnitOfWork
    {
        public BusinessUnitOfWork(SqlConnection connection) : base(connection)
        {
        }
    }
}