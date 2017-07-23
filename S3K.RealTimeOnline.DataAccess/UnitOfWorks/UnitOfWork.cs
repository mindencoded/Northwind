﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.Repositories;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected readonly SqlConnection SqlConnection;
        protected readonly SqlTransaction SqlTransaction;
        protected bool IsCommited;
        protected bool IsDisposed;
        protected IDictionary<Type, object> Repositories = new Dictionary<Type, object>();

        protected UnitOfWork(SqlConnection sqlConnection, bool isTransactional = true)
        {
            SqlConnection = sqlConnection;
            if (isTransactional)
                SqlTransaction = SqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            if (SqlTransaction != null)
            {
                SqlTransaction.Commit();
                IsCommited = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (!Repositories.Keys.Contains(typeof(IRepository<T>)))
                Repositories.Add(typeof(IRepository<T>), new Repository<T>(SqlConnection, SqlTransaction));

            return Repositories[typeof(IRepository<T>)] as IRepository<T>;
        }

        public object Repository(Type type)
        {
            if (!typeof(IRepository).IsAssignableFrom(type))
                throw new InvalidOperationException(string.Format("Type {0} not implement IRepository", type.Name));

            if (!Repositories.ContainsKey(type))
            {
                var instance = Activator.CreateInstance(type, SqlConnection, SqlTransaction);
                Repositories.Add(type, instance);
            }
            return Repositories[type];
        }

        public void Register(IRepository repository)
        {
            repository.SetSqlConnection(SqlConnection);
            repository.SetSqlTransaction(SqlTransaction);

            if (!Repositories.ContainsKey(repository.GetType()))
                Repositories.Add(repository.GetType(), repository);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                    if (SqlConnection != null)
                    {
                        if (!IsCommited)
                            SqlTransaction.Rollback();

                        SqlConnection.Close();
                    }
                IsDisposed = true;
            }
        }
    }
}