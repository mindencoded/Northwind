using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using S3K.RealTimeOnline.DataAccess.Repositories;
using S3K.RealTimeOnline.DataAccess.Tools;
using Serilog;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected readonly SqlConnection Connection;
        protected readonly SqlTransaction Transaction;
        protected bool IsCommited;
        protected bool IsDisposed;
        protected IDictionary<Type, object> Repositories = new Dictionary<Type, object>();

        protected UnitOfWork(SqlConnection connection)
        {
            Connection = connection;
            Connection.FireInfoMessageEventOnUserErrors = true;
            Connection.InfoMessage += OnInfoMessage;
            Connection.StateChange += OnStateChange;
            Transaction = Connection.BeginTransaction();
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
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
                Repositories.Add(typeof(IRepository<T>), new Repository<T>(Connection, Transaction));
            return Repositories[typeof(IRepository<T>)] as IRepository<T>;
        }

        public object Repository(Type type)
        {
            if (!typeof(IRepository).IsAssignableFrom(type))
                throw new InvalidOperationException(string.Format("Type {0} not implement IRepository", type.Name));

            if (!Repositories.ContainsKey(type))
            {
                var instance = Activator.CreateInstance(type, Connection, Transaction);
                Repositories.Add(type, instance);
            }
            return Repositories[type];
        }

        public void Register(IRepository repository)
        {
            repository.SetSqlConnection(Connection);
            repository.SetSqlTransaction(Transaction);

            if (!Repositories.ContainsKey(repository.GetType()))
                Repositories.Add(repository.GetType(), repository);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                    if (Connection != null)
                    {
                        if (!IsCommited)
                            Transaction.Rollback();

                        Connection.Close();
                    }
                IsDisposed = true;
            }
        }

        protected static void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            foreach (SqlError err in args.Errors)
            {
                Log.Information(
                    "The {0} has received a severity {1}, state {2} error number {3}\n" +
                    "on line {4} of procedure {5} on server {6}:\n{7}",
                    err.Source, err.Class, err.State, err.Number, err.LineNumber,
                    err.Procedure, err.Server, err.Message);
            }
        }

        protected static void OnStateChange(object sender, StateChangeEventArgs args)
        {
            Log.Information(
                "The current Connection state has changed from {0} to {1}.",
                args.OriginalState, args.CurrentState);
        }

        public IEnumerable<T> ExecuteQueryText<T>(string commandText, object query = null) where T : class
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                Transaction = Transaction,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (query != null)
            {
                IList<string> parameters = Regex.Matches(command.CommandText, @"\@\w+").Cast<Match>().Select(m => m.Value).ToList();
                PropertyInfo[] properties = query.GetType().GetProperties();
                foreach (var parameter in parameters)
                {
                    foreach (var property in properties)
                    {
                        string parameterName = '@' + property.Name;
                        if (parameter.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                        {
                            SqlParameter sqlParameter = new SqlParameter
                            {
                                ParameterName = parameterName,
                                SqlDbType = TypeConvertor.ToSqlDbType(property.PropertyType),
                                Value = property.GetValue(query, null)
                            };
                            command.Parameters.Add(sqlParameter);
                        }
                    }
                } 
            }

            SqlDataReader reader = command.ExecuteReader();
            IList<T> results = new List<T>();
            while (reader.Read())
            {
                results.Add(reader.ConvertToEntity<T>());
            }

            return results.AsEnumerable();
        }

        public IEnumerable<T> ExecuteQueryFunction<T>(string commandText, object query = null)
            where T : class
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                Transaction = Transaction,
                CommandType = CommandType.StoredProcedure
            };

            if (query != null)
            {
                SqlCommandBuilder.DeriveParameters(command);
                SqlParameterCollection parameters = command.Parameters;
                PropertyInfo[] properties = query.GetType().GetProperties();
                foreach (SqlParameter parameter in parameters)
                {
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        string parameterName = '@' + propertyInfo.Name;
                        if (parameterName.Equals(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
                        {
                            parameter.Value = propertyInfo.GetValue(query, null);
                        }
                    }
                }
            }

            SqlDataReader reader = command.ExecuteReader();
            IList<T> results = new List<T>();
            while (reader.Read())
            {
                results.Add(reader.ConvertToEntity<T>());
            }

            return results.AsEnumerable();
        }
    }
}