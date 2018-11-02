using System;
using System.Data.SqlClient;

namespace Northwind.WebRole.Repositories
{
    public interface IRepository : IDisposable
    {
        void SetSqlConnection(SqlConnection sqlConnection);

        void SetSqlTransaction(SqlTransaction sqlTransaction);

        bool IsOpenConnection();
    }
}