using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Repositories
{
    public class CommandRepository<T> : QueryRepository<T>, ICommandRepository<T> where T : class
    {
        public CommandRepository(SqlConnection connection) : base(connection)
        {
        }

        public CommandRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public CommandRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public CommandRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public CommandRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public CommandRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }

        public virtual T SelectById(object id)
        {
            T result = null;
            using (SqlCommand command = CreateCommandSelectById(id))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.ConvertTo<T>();
                        break;
                    }
                }
            }

            return result;
        }

        public virtual async Task<T> SelectByIdAsync(object id)
        {
            T result = null;
            using (SqlCommand command = CreateCommandSelectById(id))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = await reader.ConvertToAsync<T>();
                        break;
                    }
                }
            }

            return result;
        }

        public virtual int Insert(object parameters)
        {
            using (SqlCommand command = CreateCommandInsert(parameters))
            {
                return (int) command.ExecuteScalar();
            }
        }

        public virtual async Task<int> InsertAsync(object parameters)
        {
            using (SqlCommand command = CreateCommandInsert(parameters))
            {
                return (int) await command.ExecuteScalarAsync();
            }
        }

        public virtual int Insert(IDictionary<string, object> parameters)
        {
            using (SqlCommand command = CreateCommandInsert(parameters))
            {
                return (int) command.ExecuteScalar();
            }
        }

        public virtual async Task<int> InsertAsync(IDictionary<string, object> parameters)
        {
            using (SqlCommand command = CreateCommandInsert(parameters))
            {
                return (int) await command.ExecuteScalarAsync();
            }
        }

        public virtual int Update(object parameters)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> UpdateAsync(object parameters)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public virtual int Update(IDictionary<string, object> parameters)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> UpdateAsync(IDictionary<string, object> parameters)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public virtual int Update(object parameters, object conditions)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters, conditions))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> UpdateAsync(object parameters, object conditions)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters, conditions))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public virtual int Update(IDictionary<string, object> parameters, IDictionary<string, object> conditions)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters, conditions))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> UpdateAsync(IDictionary<string, object> parameters,
            IDictionary<string, object> conditions)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters, conditions))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public virtual int Delete(object conditions)
        {
            using (SqlCommand command = CreateCommandDelete(conditions))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> DeleteAsync(object conditions)
        {
            using (SqlCommand command = CreateCommandDelete(conditions))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public virtual int Delete(IDictionary<string, object> conditions)
        {
            using (SqlCommand command = CreateCommandDelete(conditions))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> DeleteAsync(IDictionary<string, object> conditions)
        {
            using (SqlCommand command = CreateCommandDelete(conditions))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public virtual int DeleteById(object id)
        {
            using (SqlCommand command = CreateCommandDeleteById(id))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual async Task<int> DeleteByIdAsync(object id)
        {
            using (SqlCommand command = CreateCommandDeleteById(id))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        private SqlCommand CreateCommandSelectById(object id)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateSelectByIdStatement<T>(id,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
            {
                command.Parameters.AddRange(sqlParameterList.ToArray());
            }

            return command;
        }

        private SqlCommand CreateCommandInsert(object parameters)
        {
            bool isIdentityInsert = new DbInformation(Connection, Transaction).IsIdentityInsert<T>();
            string query = new StatementBuilder(IgnoreNulls).CreateInsertStatement<T>(parameters, isIdentityInsert,
                out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandInsert(IDictionary<string, object> parameters)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateInsertStatement<T>(parameters,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandUpdate(object parameters)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateUpdateStatement<T>(parameters,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(IDictionary<string, object> parameters)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateUpdateStatement<T>(parameters,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(object parameters, object conditions)
        {
            string query = new StatementBuilder(IgnoreNulls).CreateUpdateStatement<T>(parameters, conditions,
                out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(IDictionary<string, object> parameters,
            IDictionary<string, object> conditions)
        {
            string query = new StatementBuilder(IgnoreNulls).CreateUpdateStatement<T>(parameters, conditions,
                out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandDelete(object conditions)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateDeleteStatement<T>(conditions,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandDelete(IDictionary<string, object> conditions)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateDeleteStatement<T>(conditions,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandDeleteById(object id)
        {
            string query = new StatementBuilder(IgnoreNulls).CreateDeleteStatementById<T>(id, out string propertyName);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (propertyName != null)
            {
                command.Parameters.AddWithValue(propertyName, id);
            }

            return command;
        }
    }
}