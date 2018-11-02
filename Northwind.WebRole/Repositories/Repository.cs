using System;
using System.Data;
using System.Data.SqlClient;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.Repositories
{
    public class Repository : IRepository
    {
        protected SqlConnection Connection;
        protected bool IgnoreNulls;
        protected SqlTransaction Transaction;

        public Repository(SqlConnection connection)
        {
            Connection = connection;
            IgnoreNulls = true;
        }

        public Repository(SqlConnection connection, bool ignoreNulls)
        {
            Connection = connection;
            IgnoreNulls = ignoreNulls;
        }

        public Repository(SqlConnection connection, SqlTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
            IgnoreNulls = true;
        }

        public Repository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls)
        {
            Connection = connection;
            Transaction = transaction;
            IgnoreNulls = ignoreNulls;
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            unitOfWork.Register(this);
        }

        public Repository(IUnitOfWork unitOfWork, bool ignoreNulls)
        {
            unitOfWork.Register(this);
            IgnoreNulls = ignoreNulls;
        }

        public bool IsOpenConnection()
        {
            return Connection != null && Connection.State == ConnectionState.Open;
        }

        public void SetSqlConnection(SqlConnection sqlConnection)
        {
            Connection = sqlConnection;
        }

        public void SetSqlTransaction(SqlTransaction sqlTransaction)
        {
            Transaction = sqlTransaction;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (Connection != null)
                    Connection.Close();
        }
    }
}