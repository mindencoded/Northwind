using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Repositories
{
    public class QueryRepository<T> : Repository, IQueryRepository<T> where T : class
    {

        public QueryRepository(SqlConnection connection) : base(connection)
        {
        }

        public QueryRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public QueryRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public QueryRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public QueryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public QueryRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }

        public virtual IEnumerable<ExpandoObject> Select(IList<string> columns, string orderBy = null, int? page = null,
            int? pageSize = null)
        {
            using (SqlCommand command = CreateCommandSelect(columns, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicList();
                }
            }
        }

        public virtual async Task<IEnumerable<ExpandoObject>> SelectAsync(IList<string> columns, string orderBy = null,
            int? page = null, int? pageSize = null)
        {
            using (SqlCommand command = CreateCommandSelect(columns, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ConvertToDynamicListAsync();
                }
            }
        }

        public virtual IEnumerable<T> Select(IDictionary<string, object> conditions, string orderBy = null,
            int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(null, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToList<T>();
                }
            }
        }

        public virtual async Task<IEnumerable<T>> SelectAsync(IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(null, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ConvertToListAsync<T>();
                }
            }
        }

        public virtual IEnumerable<T> Select(IList<ParameterBuilder> conditions, string orderBy = null,
            int? page = null,
            int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(null, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToList<T>();
                }
            }
        }

        public virtual async Task<IEnumerable<T>> SelectAsync(IList<ParameterBuilder> conditions, string orderBy = null,
            int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(null, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ConvertToListAsync<T>();
                }
            }
        }

        public virtual IEnumerable<ExpandoObject> Select(IList<string> columns, IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(columns, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicList();
                }
            }
        }

        public virtual async Task<IEnumerable<ExpandoObject>> SelectAsync(IList<string> columns,
            IDictionary<string, object> conditions, string orderBy = null, int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(columns, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ConvertToDynamicListAsync();
                }
            }
        }

        public virtual IEnumerable<ExpandoObject> Select(IList<string> columns, IList<ParameterBuilder> conditions,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(columns, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicList();
                }
            }
        }

        public virtual async Task<IEnumerable<ExpandoObject>> SelectAsync(IList<string> columns,
            IList<ParameterBuilder> conditions, string orderBy = null, int? page = null, int? pageSize = null)
        {
            using (SqlCommand command =
                CreateCommandSelect(columns, conditions, orderBy, page, pageSize))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ConvertToDynamicListAsync();
                }
            }
        }

        public virtual int Count(IDictionary<string, object> conditions)
        {
            using (SqlCommand command =
                CreateCommandCount(conditions))
            {
                return (int) command.ExecuteScalar();
            }
        }

        public virtual int Count(IList<ParameterBuilder> conditions)
        {
            using (SqlCommand command =
                CreateCommandCount(conditions))
            {
                return (int) command.ExecuteScalar();
            }
        }

        public virtual async Task<int> CountAsync(IDictionary<string, object> conditions)
        {
            using (SqlCommand command =
                CreateCommandCount(conditions))
            {
                return (int) await command.ExecuteScalarAsync();
            }
        }

        public virtual async Task<int> CountAsync(IList<ParameterBuilder> conditions)
        {
            using (SqlCommand command =
                CreateCommandCount(conditions))
            {
                return (int) await command.ExecuteScalarAsync();
            }
        }

        private SqlCommand CreateCommandSelect(IList<string> columns, string orderBy = null, int? page = null,
            int? pageSize = null)
        {
            string query = new StatementBuilder(IgnoreNulls).CreateSelectStatement<T>(columns,
                out IList<SqlParameter> sqlParameterList, orderBy, page,
                pageSize);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandSelect(IList<string> columns, IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            string query = new StatementBuilder(IgnoreNulls).CreateSelectStatement<T>(columns, conditions,
                out IList<SqlParameter> sqlParameterList, orderBy,
                page, pageSize);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandSelect(IList<string> columns, IList<ParameterBuilder> conditions,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            string query = new StatementBuilder(IgnoreNulls).CreateSelectStatement<T>(columns, conditions,
                out IList<SqlParameter> sqlParameterList, orderBy,
                page, pageSize);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandCount(IDictionary<string, object> conditions)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateCountStatement<T>(conditions,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandCount(IList<ParameterBuilder> conditions)
        {
            string query =
                new StatementBuilder(IgnoreNulls).CreateCountStatement<T>(conditions,
                    out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }
    }
}