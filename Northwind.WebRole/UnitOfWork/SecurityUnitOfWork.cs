using System.Data.SqlClient;

namespace Northwind.WebRole.UnitOfWork
{
    public class SecurityUnitOfWork : UnitOfWork, ISecurityUnitOfWork
    {
        public SecurityUnitOfWork(SqlConnection connection) : base(connection)
        {
        }
    }
}