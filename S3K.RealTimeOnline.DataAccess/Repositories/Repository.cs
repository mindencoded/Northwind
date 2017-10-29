using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using S3K.RealTimeOnline.DataAccess.Tools;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.Domain;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected SqlConnection Connection;
        protected SqlTransaction Transaction;
        protected bool IgnoreNulls;
        private readonly IEnumerable<ColumnAttribute> _columnAttributes;

        public Repository(SqlConnection connection)
        {
            Connection = connection;
            IgnoreNulls = true;
            _columnAttributes = typeof(T).GetProperties().Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(SqlConnection connection, bool ignoreNulls)
        {
            Connection = connection;
            IgnoreNulls = ignoreNulls;
            _columnAttributes = typeof(T).GetProperties().Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(SqlConnection connection, SqlTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
            IgnoreNulls = true;
            _columnAttributes = typeof(T).GetProperties().Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls)
        {
            Connection = connection;
            Transaction = transaction;
            IgnoreNulls = ignoreNulls;
            _columnAttributes = typeof(T).GetProperties().Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            unitOfWork.Register(this);
            _columnAttributes = typeof(T).GetProperties().Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(IUnitOfWork unitOfWork, bool ignoreNulls)
        {
            unitOfWork.Register(this);
            IgnoreNulls = ignoreNulls;
            _columnAttributes = typeof(T).GetProperties().Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public IEnumerable<dynamic> Select(IList<string> columns)
        {
            using (SqlCommand command = CreateCommandSelect(columns))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicEnumerable();
                }
            }
        }

        public virtual IEnumerable<T> Select(object conditions)
        {
            using (SqlCommand command = CreateCommandSelect(null, conditions))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToEnumerable<T>();
                }
            }
        }

        public virtual IEnumerable<T> Select(IDictionary<string, object> conditions)
        {
            using (SqlCommand command = CreateCommandSelect(null, conditions))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToEnumerable<T>();
                }
            }
        }

        public virtual IEnumerable<dynamic> Select(IList<string> columns, object conditions)
        {
            using (SqlCommand command = CreateCommandSelect(columns, conditions))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicEnumerable();
                }
            }
        }

        public virtual IEnumerable<dynamic> Select(IList<string> columns, IDictionary<string, object> conditions)
        {
            using (SqlCommand command = CreateCommandSelect(columns, conditions))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicEnumerable();
                }
            }
        }

        public virtual IEnumerable<dynamic> Select(IList<string> columns, object conditions, IList<string> orderBy)
        {
            using (SqlCommand command = CreateCommandSelect(columns, conditions, CreateOrderByStatement(orderBy)))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicEnumerable();
                }
            }
        }

        public virtual IEnumerable<dynamic> Select(IList<string> columns, IDictionary<string, object> conditions, IList<string> orderBy)
        {
            using (SqlCommand command = CreateCommandSelect(columns, conditions, CreateOrderByStatement(orderBy)))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToDynamicEnumerable();
                }
            }
        }

        public IEnumerable<T> Select(object conditions, IList<string> orderBy)
        {
            using (SqlCommand command = CreateCommandSelect(null, conditions, CreateOrderByStatement(orderBy)))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToEnumerable<T>();
                }
            }
        }

        public IEnumerable<T> Select(IDictionary<string, object> conditions, IList<string> orderBy)
        {
            using (SqlCommand command = CreateCommandSelect(null, conditions, CreateOrderByStatement(orderBy)))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.ConvertToEnumerable<T>();
                }
            }
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

        public virtual int Insert(object parameters)
        {
            using (SqlCommand command = CreateCommandInsert(parameters))
            {
                return (int) command.ExecuteScalar();
            }
        }

        public virtual int Update(object parameters)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual int Update(object parameters, object conditions)
        {
            using (SqlCommand command = CreateCommandUpdate(parameters, conditions))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual int Delete(object conditions)
        {
            using (SqlCommand command = CreateCommandDelete(conditions))
            {
                return command.ExecuteNonQuery();
            }
        }

        public virtual int DeleteById(object id)
        {
            using (SqlCommand command = CreateCommandDeleteById(id))
            {
                return command.ExecuteNonQuery();
            }
        }

        public bool IsOpenConnection()
        {
            return Connection != null && Connection.State == ConnectionState.Open;
        }

        public bool IsIdentityInsert()
        {
            string query = @"SELECT OBJECTPROPERTY(OBJECT_ID('" + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]'), 'TableHasIdentity');";
            SqlCommand command = new SqlCommand(query, Connection, Transaction);
            return (int) command.ExecuteScalar() == 1;
        }

        public void SetSqlConnection(SqlConnection sqlConnection)
        {
            Connection = sqlConnection;
        }

        public void SetSqlTransaction(SqlTransaction sqlTransaction)
        {
            Transaction = sqlTransaction;
        }

        public SqlDataAdapter SqlDataAdapter()
        {
            
            string selectCmdText = CreateSelectStatement();
            string insertCmdText = CreateInsertStatement(out IList<SqlParameter> insertSqlParameterList);
            string updateCmdText = CreateUpdateStatement(out IList<SqlParameter> updateSqlParameterList);
            string deleteCmdText = CreateDeleteStatement(out IList<SqlParameter> deleteSqlParameterList);
            SqlDataAdapter adapter = new SqlDataAdapter
            {
                MissingSchemaAction = MissingSchemaAction.AddWithKey
            };

            if (Transaction != null)
            {
                adapter.SelectCommand = new SqlCommand(selectCmdText, Connection, Transaction);
                adapter.InsertCommand = new SqlCommand(insertCmdText, Connection, Transaction);
                adapter.UpdateCommand = new SqlCommand(updateCmdText, Connection, Transaction);
                adapter.DeleteCommand = new SqlCommand(deleteCmdText, Connection, Transaction);
            }
            else
            {
                adapter.SelectCommand = new SqlCommand(selectCmdText, Connection);
                adapter.InsertCommand = new SqlCommand(insertCmdText, Connection);
                adapter.UpdateCommand = new SqlCommand(updateCmdText, Connection);
                adapter.DeleteCommand = new SqlCommand(deleteCmdText, Connection);
            }

            if (insertSqlParameterList.Count > 0)
            {
                adapter.InsertCommand.Parameters.AddRange(insertSqlParameterList.ToArray());
            }

            if (updateSqlParameterList.Count > 0)
            {
                adapter.UpdateCommand.Parameters.AddRange(updateSqlParameterList.ToArray());
            }

            if (deleteSqlParameterList.Count > 0)
            {
                adapter.DeleteCommand.Parameters.AddRange(deleteSqlParameterList.ToArray());
            }
            return adapter;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (Connection != null)
                    Connection.Close();
        }

        private string CreateSelectStatement(IList<string> columns = null,  IList<SqlParameter> sqlParameterList = null, string orderBy = null)
        {
            IList<string> conditionList = new List<string>();

            if (sqlParameterList != null)
            {
                foreach (SqlParameter sqlParameter in sqlParameterList)
                {
                    if (sqlParameter.SqlDbType == SqlDbType.VarChar)
                    {
                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" +
                                          EntityUtils.GetTableName<T>() +
                                          "].[" + sqlParameter.SourceColumn + "] LIKE '%' + " +
                                          sqlParameter.ParameterName +
                                          " + '%'");
                    }
                    else
                    {
                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" +
                                          EntityUtils.GetTableName<T>() +
                                          "].[" + sqlParameter.SourceColumn + "] = " + sqlParameter.ParameterName);
                    }
                }
            }
            string columnsStr = columns != null ? EntityUtils.JoinColumns<T>(columns, true) : EntityUtils.JoinColumns<T>();
            string query = @"SELECT " + columnsStr + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]";

            if (conditionList.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditionList);
            }

            if (orderBy != null)
            {
                query += " ORDER BY " + orderBy;
            }

            return query;
        }

        private string CreateInsertStatement(out IList<SqlParameter> sqlParameterList)
        {
            sqlParameterList = new List<SqlParameter>();
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> columnList = new List<string>();
            foreach (PropertyInfo typeProperty in typeProperties)
            {
                Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null && IsIdentityInsert())
                {
                    continue;
                }
                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = '@' + typeProperty.Name,
                    SourceColumn = columnName,
                    SqlDbType = TypeConvertor.ToSqlDbType(typeProperty.PropertyType)
                };
                
                columnList.Add("[" + columnName + "]");
                sqlParameterList.Add(sqlParameter);
            }
            return @"INSERT INTO " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] (" +
                   string.Join(", ", columnList) + ") VALUES(" +
                   string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                   ");SELECT CAST(SCOPE_IDENTITY() AS int);";
        }

        private string CreateUpdateStatement(out IList<SqlParameter> sqlParameterList)
        {
            sqlParameterList = new List<SqlParameter>();
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameters = new List<string>();
            IList<string> conditions = new List<string>();
            foreach (PropertyInfo typeProperty in typeProperties)
            {

                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;
                Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                string parameterName = '@' + typeProperty.Name;
                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
                    SourceColumn = columnName,
                    SqlDbType = TypeConvertor.ToSqlDbType(typeProperty.PropertyType)
                };

                if (keyAttribute != null)
                {
                    conditions.Add("[" + columnName + "] = " + parameterName);
                    sqlParameter.SourceVersion = DataRowVersion.Original;
                }
                else
                {
                    parameters.Add("[" + columnName + "] = " + parameterName);
                }

              

                sqlParameterList.Add(sqlParameter);
            }
            return @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] SET " +
                   string.Join(", ", parameters) + " WHERE " + string.Join(" AND ", conditions);
        }

        private string CreateDeleteStatement(out IList<SqlParameter> sqlParameterList)
        {
            sqlParameterList = new List<SqlParameter>();
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> conditionList = new List<string>();
           
            foreach (PropertyInfo typeProperty in typeProperties)
            {
                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;
                Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null)
                {

                    conditionList.Add("[" + columnName + "] = " + '@' + typeProperty.Name);
                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = '@' + typeProperty.Name,
                        SourceColumn = columnName,
                        SqlDbType = TypeConvertor.ToSqlDbType(typeProperty.PropertyType)
                    };
                   
                    sqlParameterList.Add(sqlParameter);
                }
            }
            return @"DELETE FROM " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] WHERE " + string.Join(" AND ", conditionList);
        }

        private IList<SqlParameter> CreateSelectParameters(object conditions)
        {
            IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            PropertyInfo[] properties = conditions.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }

                object value = property.GetValue(conditions, null);

                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);
                ColumnAttribute columnAttribute = typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>()
                    .FirstOrDefault();
                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;
                string parameterName = '@' + typeProperty.Name;
               
                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
                    SourceColumn = columnName,
                    SqlDbType = TypeConvertor.ToSqlDbType(typeProperty.PropertyType),
                    Value = value ?? DBNull.Value
                };

                sqlParameterList.Add(sqlParameter);
            }
            return sqlParameterList;
        }

        private IList<SqlParameter> CreateSelectParameters(IDictionary<string, object> conditions)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
           
            foreach (KeyValuePair<string, object> entry in conditions)
            {
                object value = entry.Value;
                if (value == null && IgnoreNulls)
                {
                    continue;
                }
                string columnName = null;
                string propertyName = entry.Key.TrimStart('@');//entry.Key.Replace("@", "").Trim();

                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    if (_columnAttributes.Select(x => x.Name).Contains(propertyName))
                    {
                        columnName = propertyName;
                    }
                }

                if (columnName == null)
                {
                    continue;
                }

                string parameterName = '@' + propertyName;

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
                    SourceColumn = columnName,
                    Value = value ?? DBNull.Value
                };

                if (value != null)
                {
                    sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                }

                sqlParameterList.Add(sqlParameter);
            }
            return sqlParameterList;
        }

        private SqlCommand CreateCommandSelect(IList<string> columns)
        {          
            string query = CreateSelectStatement(columns);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            return command;
        }

        private SqlCommand CreateCommandSelect(IList<string> columns, object conditions, string orderBy = null)
        {
            IList<SqlParameter> sqlParameterList = CreateSelectParameters(conditions);
            string query = CreateSelectStatement(columns, sqlParameterList, orderBy);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandSelect(IList<string> columns, IDictionary<string, object> conditions, string orderBy = null)
        {
            IList<SqlParameter>  sqlParameterList = CreateSelectParameters(conditions);
            string query = CreateSelectStatement(columns, sqlParameterList, orderBy);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandSelectById(object id)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            string parameterName = null;
            Attribute keyAttribute = null;
            string columnName = null;
            foreach (PropertyInfo property in properties)
            {
                parameterName = '@' + property.Name;
                keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null)
                {
                    ColumnAttribute columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : property.Name;
                    break;
                }
            }

            if (keyAttribute == null)
            {
                throw new InvalidOperationException("The entity has not established a KeyAttribute.");
            }

            string query = @"SELECT " + EntityUtils.JoinColumns<T>() + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "] WHERE " + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() +
                           "].[" + columnName + "] = " + parameterName;

            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = parameterName,
                SourceColumn = columnName,
                SqlDbType = TypeConvertor.ToSqlDbType(id.GetType()),
                Value = id
            };
            command.Parameters.Add(sqlParameter);
            return command;
        }

        private SqlCommand CreateCommandInsert(object parameters)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> columnNameList = new List<string>();
            IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
            PropertyInfo[] properties = parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }

                PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);

                Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null && IsIdentityInsert())
                {
                    continue;
                }

                object value = property.GetValue(parameters, null);
                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                string parameterName = '@' + typeProperty.Name;
                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;
                columnNameList.Add("[" + columnName + "]");

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
                    SourceColumn = columnName
                };

                if (value != null)
                {
                    sqlParameter.Value = value;
                    sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                }
                else
                {
                    sqlParameter.Value = DBNull.Value;
                }

                sqlParameterList.Add(sqlParameter);
            }

            string query = @"INSERT INTO " + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "] (" +
                           string.Join(", ", columnNameList) + ") VALUES(" +
                           string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                           "); SELECT CAST(SCOPE_IDENTITY() AS int);";

            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandUpdate(object parameters)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
            PropertyInfo[] properties = parameters.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }

                object value = property.GetValue(parameters, null);

                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);

                string parameterName = '@' + typeProperty.Name;
                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));

                if (keyAttribute != null)
                    conditionList.Add("[" + columnName + "] = " + parameterName);
                else
                    parameterList.Add("[" + columnName + "] = " + parameterName);

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
                    SourceColumn = columnName
                };

                if (value != null)
                {
                    sqlParameter.Value = value;
                    sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                }
                else
                {
                    sqlParameter.Value = DBNull.Value;
                }

                sqlParameterList.Add(sqlParameter);
            }


            if (parameterList.Count == 0)
            {
                throw new InvalidOperationException("Parameters list is empty.");
            }


            if (conditionList.Count == 0)
            {
                throw new InvalidOperationException("Conditions list is empty.");
            }

            string query = @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "] SET " +
                           string.Join(", ", parameterList) + " WHERE " + string.Join(" AND ", conditionList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(object parameters, object conditions)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
            int parameterCounter = 1;
            PropertyInfo[] properties = parameters.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }
                PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);
                string parameterName = '@' + typeProperty.Name;

                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                object value = property.GetValue(parameters, null);

                parameterList.Add("[" + columnName + "] = " + parameterName);

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
                    SourceColumn = columnName
                };

                if (value != null)
                {
                    sqlParameter.Value = value;
                    sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                }
                else
                {
                    sqlParameter.Value = DBNull.Value;
                }

                sqlParameterList.Add(sqlParameter);
            }

            if (conditions != null)
            {
                properties = conditions.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                    {
                        continue;
                    }

                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);
                    object value = property.GetValue(conditions, null);

                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                    string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                    string parameterName = '@' + typeProperty.Name;
                    if (sqlParameterList.Select(x => x.ParameterName).Contains(parameterName))
                    {
                        parameterName += "_" + parameterCounter;
                        parameterCounter++;
                    }
                    conditionList.Add("[" + columnName + "] = " + parameterName);

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameterName,
                        SourceColumn = columnName
                    };

                    if (value != null)
                    {
                        sqlParameter.Value = value;
                        sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                    }
                    else
                    {
                        sqlParameter.Value = DBNull.Value;
                    }

                    sqlParameterList.Add(sqlParameter);
                }
            }

            if (parameterList.Count == 0)
            {
                throw new InvalidOperationException("Parameters list is empty.");
            }

            string query = @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "] SET " +
                           string.Join(", ", parameterList);

            if (conditionList.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditionList);
            }

            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandDelete(object conditions)
        {
            IList<string> conditionList = new List<string>();
            IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
            if (conditions != null)
            {
                Type type = typeof(T);
                PropertyInfo[] typeProperties = type.GetProperties();
                PropertyInfo[] properties = conditions.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                    {
                        continue;
                    }

                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);
                    object value = property.GetValue(conditions, null);

                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    string parameterName = '@' + typeProperty.Name;

                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                    string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                    conditionList.Add(EntityUtils.GetSchema<T>() + ".[" +
                                      EntityUtils.GetTableName<T>() +
                                      "].[" + columnName + "] = " + parameterName);

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameterName,
                        SourceColumn = columnName
                    };

                    if (value != null)
                    {
                        sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                        sqlParameter.Value = value;
                    }
                    else
                    {
                        sqlParameter.Value = DBNull.Value;
                    }

                    sqlParameterList.Add(sqlParameter);
                }
            }

            string query = @"DELETE FROM " + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]";

            if (conditionList.Count > 0)
                query += " WHERE " + string.Join(" AND ", conditionList);

            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandDeleteById(object id)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            string condition = null;
            string propertyName = null;
            if (id != null)
            {
                foreach (PropertyInfo typeProperty in typeProperties)
                {
                    Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                    if (keyAttribute == null) continue;
                    propertyName = '@' + typeProperty.Name;
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;
                    condition = EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                "].[" + columnName + "] = " + propertyName;
                    break;
                }
            }
            string query = @"DELETE FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]";
            if (condition != null)
            {
                query += " WHERE " + condition;
            }

            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (propertyName != null)
            {
                command.Parameters.AddWithValue(propertyName, id);
            }

            return command;
        }

        private string CreateOrderByStatement(IList<string> orderBy)
        {
            string[] propertyNames = typeof(T).GetProperties().Select(x => x.Name).ToArray();
            IList<string> columnNames = new List<string>();
            foreach (string orderItem in orderBy)
            {
                string propertyName;
                string direction = null;
                string columnName = null;
                if (orderItem.Split(' ').Length > 1)
                {
                    propertyName = orderItem.Split(' ')[0];
                    string ascOrDesc = orderItem.Split(' ')[1].ToUpper();
                    if (ascOrDesc.Equals("DESC") || ascOrDesc.Equals("ASC"))
                    {
                        direction = ascOrDesc;
                    }
                }
                else
                {
                    propertyName = orderItem;
                }
                
                if (propertyNames.Contains(propertyName))
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        ColumnAttribute columnAttribute = propertyInfo.GetCustomAttributes(false)
                            .OfType<ColumnAttribute>()
                            .FirstOrDefault();
                        if (columnAttribute != null)
                        {
                            columnName = columnAttribute.Name;
                        }
                        else
                        {
                            columnName = propertyName;
                        }
                    }
                }
                else
                {
                    if (_columnAttributes.Select(x => x.Name).Contains(propertyName))
                    {
                        columnName = propertyName;
                    }
                }

                if (columnName != null)
                {
                    if (direction != null)
                    {
                        columnName += ' ' + direction;
                    }

                    columnNames.Add(columnName);
                }
            }

            if (columnNames.Count > 0)
            {
                return string.Join(", ", columnNames);
            }

            throw new InvalidOperationException("Error to create 'Order By' secuence.");
        }
    }
}