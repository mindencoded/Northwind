using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Repositories;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.UnitOfWork
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private static readonly TraceSource Trace = new TraceSource(typeof(UnitOfWork).Name);
        protected readonly SqlConnection Connection;
        protected bool IsCommited;
        protected bool IsDisposed;
        protected IDictionary<Type, object> Repositories = new Dictionary<Type, object>();
        protected SqlTransaction Transaction;

        protected UnitOfWork(SqlConnection connection)
        {
            Connection = connection;
            Initializer();
        }

        protected UnitOfWork(SqlConnection connection, bool isTransactionable)
        {
            Connection = connection;
            if (isTransactionable)
            {
                Transaction = Connection.BeginTransaction();
            }

            Initializer();
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                IsCommited = true;
            }
        }

        public void BeginTransaction()
        {
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        public void Close()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        public void Open()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        public async Task OpenAsync()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                await Connection.OpenAsync();
            }
        }

        public bool IsOpen()
        {
            return Connection.State == ConnectionState.Open;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (!Repositories.Keys.Contains(typeof(IRepository<T>)))
            {
                Repositories.Add(typeof(IRepository<T>),
                    Transaction != null ? new Repository<T>(Connection, Transaction) : new Repository<T>(Connection));
            }

            return Repositories[typeof(IRepository<T>)] as IRepository<T>;
        }

        public object Repository(Type type)
        {
            if (!typeof(IRepository).IsAssignableFrom(type))
                throw new InvalidOperationException(string.Format("Type {0} not implement IRepository", type.Name));

            if (!Repositories.ContainsKey(type))
            {
                object instance = Transaction != null
                    ? Activator.CreateInstance(type, Connection, Transaction)
                    : Activator.CreateInstance(type, Connection);
                Repositories.Add(type, instance);
            }

            return Repositories[type];
        }

        public void Register(IRepository repository)
        {
            repository.SetSqlConnection(Connection);
            if (Transaction != null)
            {
                repository.SetSqlTransaction(Transaction);
            }

            if (!Repositories.ContainsKey(repository.GetType()))
                Repositories.Add(repository.GetType(), repository);
        }

        public virtual T ExecuteQuery<T>(string commandText, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetQueryParameters(command, values);
            }

            return ExecuteCommand<T>(command);
        }

        public virtual async Task<T> ExecuteQueryAsync<T>(string commandText, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetQueryParameters(command, values);
            }

            return await ExecuteCommandAsync<T>(command);
        }

        public virtual T ExecuteQuery<T>(string commandText, IDictionary<string, object> values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Count > 0)
            {
                SetParameters(command, values);
            }

            return ExecuteCommand<T>(command);
        }

        public virtual async Task<T> ExecuteQueryAsync<T>(string commandText, IDictionary<string, object> values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Count > 0)
            {
                SetParameters(command, values);
            }

            return await ExecuteCommandAsync<T>(command);
        }

        public virtual T ExecuteFunction<T>(string commandText, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetFunctionParameters(command, values);
            }

            return ExecuteCommand<T>(command);
        }

        public virtual async Task<T> ExecuteFunctionAsync<T>(string commandText, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetFunctionParameters(command, values);
            }

            return await ExecuteCommandAsync<T>(command);
        }

        public virtual T ExecuteFunction<T>(string commandText, IDictionary<string, object> values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Count > 0)
            {
                SetParameters(command, values);
            }

            return ExecuteCommand<T>(command);
        }

        public virtual async Task<T> ExecuteFunctionAsync<T>(string commandText, IDictionary<string, object> values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Count > 0)
            {
                SetParameters(command, values);
            }

            return await ExecuteCommandAsync<T>(command);
        }

        public virtual int ExecuteNonQuery(string commandText, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetFunctionParameters(command, values);
            }

            return command.ExecuteNonQuery();
        }

        public virtual async Task<int> ExecuteNonQueryAsync(string commandText, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetFunctionParameters(command, values);
            }

            return await command.ExecuteNonQueryAsync();
        }

        public virtual int ExecuteNonQuery(string commandText, IDictionary<string, object> values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Count > 0)
            {
                SetParameters(command, values);
            }

            return command.ExecuteNonQuery();
        }

        public virtual async Task<int> ExecuteNonQueryAsync(string commandText, IDictionary<string, object> values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Count > 0)
            {
                SetParameters(command, values);
            }

            return await command.ExecuteNonQueryAsync();
        }

        public virtual Tuple<int, int> ExecuteNonQuery(string commandText, string returnParameterName,
            string outputParameterName, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetFunctionParameters(command, values);
            }

            SqlParameter returnSqlParameter = command.Parameters.Add(returnParameterName, SqlDbType.Int);
            returnSqlParameter.Direction = ParameterDirection.ReturnValue;

            SqlParameter outputSqlParameter = command.Parameters.Add(outputParameterName, SqlDbType.Int);
            outputSqlParameter.Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            int returnValue = (int) command.Parameters[returnParameterName].Value;
            int outPutValue = (int) command.Parameters[outputParameterName].Value;

            return new Tuple<int, int>(returnValue, outPutValue);
        }

        public virtual async Task<Tuple<int, int>> ExecuteNonQueryAsync(string commandText, string returnParameterName,
            string outputParameterName, params object[] values)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            if (values != null && values.Length > 0)
            {
                SetFunctionParameters(command, values);
            }

            SqlParameter returnSqlParameter = command.Parameters.Add(returnParameterName, SqlDbType.Int);
            returnSqlParameter.Direction = ParameterDirection.ReturnValue;

            SqlParameter outputSqlParameter = command.Parameters.Add(outputParameterName, SqlDbType.Int);
            outputSqlParameter.Direction = ParameterDirection.Output;

            await command.ExecuteNonQueryAsync();

            int returnValue = (int) command.Parameters[returnParameterName].Value;
            int outPutValue = (int) command.Parameters[outputParameterName].Value;

            return new Tuple<int, int>(returnValue, outPutValue);
        }

        public int NextIdentity<T>() where T : class
        {
            string commandText = @"SELECT CASE WHEN(SELECT COUNT(1) FROM " + EntityUtils.GetSchema<T>() + ".[" +
                                 EntityUtils.GetTableName<T>() + "]) = 0 THEN 1 ELSE IDENT_CURRENT('" +
                                 EntityUtils.GetSchema<T>() + "." + EntityUtils.GetTableName<T>() +
                                 "') + 1 END AS NextIdentity;";

            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            return (int) command.ExecuteScalar();
        }

        public async Task<int> NextIdentityAsync<T>() where T : class
        {
            string commandText = @"SELECT CASE WHEN(SELECT COUNT(1) FROM " + EntityUtils.GetSchema<T>() + ".[" +
                                 EntityUtils.GetTableName<T>() + "]) = 0 THEN 1 ELSE IDENT_CURRENT('" +
                                 EntityUtils.GetSchema<T>() + "." + EntityUtils.GetTableName<T>() +
                                 "') + 1 END AS NextIdentity;";

            SqlCommand command = new SqlCommand
            {
                Connection = Connection,
                CommandText = commandText,
                CommandType = CommandType.Text
            };

            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

            return (int) await command.ExecuteScalarAsync();
        }

        private void Initializer()
        {
            Connection.FireInfoMessageEventOnUserErrors = true;
            Connection.InfoMessage += OnInfoMessage;
            Connection.StateChange += OnStateChange;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                    if (Connection != null)
                    {
                        if (Transaction != null && !IsCommited)
                            Transaction.Rollback();

                        Connection.Close();
                    }

                IsDisposed = true;
            }
        }

        private void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            foreach (SqlError err in args.Errors)
            {
                Trace.TraceInformation(
                    "The {0} has received a severity {1}, state {2} error number {3}\n" +
                    "on line {4} of procedure {5} on server {6}:\n{7}",
                    err.Source, err.Class, err.State, err.Number, err.LineNumber,
                    err.Procedure, err.Server, err.Message);
            }
        }

        private void OnStateChange(object sender, StateChangeEventArgs args)
        {
            Trace.TraceInformation(
                "The current Connection state has changed from {0} to {1}.",
                args.OriginalState, args.CurrentState);
        }

        private T ExecuteCommand<T>(SqlCommand command)
        {
            Type returnType = typeof(T);
            T returnInstance = default(T);

            if (returnType.IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(IList<>) ||
                returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                returnType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                SqlDataReader reader = command.ExecuteReader();
                Type entityType = returnType.GetGenericArguments()[0];
                Type sqlDataReaderType = typeof(SqlDataReaderExtensions);
                MethodInfo convertToMethod =
                    sqlDataReaderType.GetMethod("ConvertTo", BindingFlags.Public | BindingFlags.Static);
                if (convertToMethod != null)
                {
                    MethodInfo convertToGenericMethod = convertToMethod.MakeGenericMethod(entityType);
                    Type listType = typeof(List<>);
                    Type constructedListType = listType.MakeGenericType(entityType);
                    IList listInstance = (IList) Activator.CreateInstance(constructedListType, null);
                    while (reader.Read())
                    {
                        object entity = convertToGenericMethod.Invoke(reader, new object[] {reader});
                        listInstance.Add(entity);
                    }

                    returnInstance = (T) listInstance;
                }
            }
            else
            {
                returnInstance = (T) command.ExecuteScalar();
            }

            return returnInstance;
        }

        private async Task<T> ExecuteCommandAsync<T>(SqlCommand command)
        {
            Type returnType = typeof(T);
            T returnInstance = default(T);

            if (returnType.IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(IList<>) ||
                returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                returnType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                SqlDataReader reader = command.ExecuteReader();
                Type entityType = returnType.GetGenericArguments()[0];
                Type sqlDataReaderType = typeof(SqlDataReaderExtensions);
                MethodInfo convertToMethod =
                    sqlDataReaderType.GetMethod("ConvertTo", BindingFlags.Public | BindingFlags.Static);
                if (convertToMethod != null)
                {
                    MethodInfo convertToGenericMethod = convertToMethod.MakeGenericMethod(entityType);
                    Type listType = typeof(List<>);
                    Type constructedListType = listType.MakeGenericType(entityType);
                    IList listInstance = (IList) Activator.CreateInstance(constructedListType, null);
                    while (await reader.ReadAsync())
                    {
                        object entity = convertToGenericMethod.Invoke(reader, new object[] {reader});
                        listInstance.Add(entity);
                    }

                    returnInstance = (T) listInstance;
                }
            }
            else
            {
                returnInstance = (T) await command.ExecuteScalarAsync();
            }

            return returnInstance;
        }

        private void SetFunctionParameters(SqlCommand command, object[] values)
        {
            SqlCommandBuilder.DeriveParameters(command);
            SqlParameterCollection sqlParameterCollection = command.Parameters;

            if (sqlParameterCollection.Count > 0)
            {
                int index = 0;
                foreach (SqlParameter sqlParameter in sqlParameterCollection)
                {
                    if (sqlParameter.Direction == ParameterDirection.Input ||
                        sqlParameter.Direction == ParameterDirection.InputOutput)
                    {
                        sqlParameter.Value = values[index];
                        index++;
                    }
                }
            }
        }

        private void SetQueryParameters(SqlCommand command, object[] values)
        {
            string[] parameters = Regex.Matches(command.CommandText, @"\@\w+").Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameters[i],
                        SqlDbType = TypeConvertor.ToSqlDbType(values[i].GetType()),
                        Value = values[i]
                    };
                    command.Parameters.Add(sqlParameter);
                }
            }
        }

        private void SetParameters(SqlCommand command, IDictionary<string, object> values)
        {
            foreach (KeyValuePair<string, object> entry in values)
            {
                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = entry.Key,
                    SqlDbType = TypeConvertor.ToSqlDbType(entry.Value.GetType()),
                    Value = entry.Value
                };
                command.Parameters.Add(sqlParameter);
            }
        }
    }
}