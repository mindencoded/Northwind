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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected SqlConnection SqlConnection;
        protected SqlTransaction SqlTransaction;
        protected bool IgnoreNulls;

        public Repository(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            IgnoreNulls = true;
        }

        public Repository(SqlConnection sqlConnection, bool ignoreNulls)
        {
            SqlConnection = sqlConnection;
            IgnoreNulls = ignoreNulls;
        }

        public Repository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            SqlConnection = sqlConnection;
            SqlTransaction = sqlTransaction;
            IgnoreNulls = true;
        }

        public Repository(SqlConnection sqlConnection, SqlTransaction sqlTransaction, bool ignoreNulls)
        {
            SqlConnection = sqlConnection;
            SqlTransaction = sqlTransaction;
            IgnoreNulls = ignoreNulls;
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            unitOfWork.Register(this);
        }

        public virtual IEnumerable<TEntity> Select(object conditions)
        {
            IList<TEntity> results;
            using (var sqlCommand = CreateCommandSelect(conditions))
            {
                using (var sqlDataReader = sqlCommand.ExecuteReader())
                {
                    results = new List<TEntity>();
                    while (sqlDataReader.Read())
                        results.Add(sqlDataReader.ConvertToEntity<TEntity>());
                }
            }
            return results;
        }

        public virtual TEntity SelectById(object id)
        {
            TEntity result = null;
            using (var sqlCommand = CreateCommandSelectById(id))
            {
                using (var sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        result = sqlDataReader.ConvertToEntity<TEntity>();
                        break;
                    }
                }
            }
            return result;
        }

        public virtual int Insert(object parameters)
        {
            var sqlCommand = CreateCommandInsert(parameters);
            return (int) sqlCommand.ExecuteScalar();
        }

        public virtual int Update(object parameters)
        {
            var sqlCommand = CreateCommandUpdate(parameters);
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Update(object parameters, object conditions)
        {
            var sqlCommand = CreateCommandUpdate(parameters, conditions);
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Delete(object conditions)
        {
            var sqlCommand = CreateDelete(conditions);
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int DeleteById(object id)
        {
            var sqlCommand = CreateDeleteById(id);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            return sqlCommand.ExecuteNonQuery();
        }

        public bool IsOpenConnection()
        {
            return SqlConnection != null && SqlConnection.State == ConnectionState.Open;
        }

        public bool IsIdentityInsert()
        {
            var sqlQuery = @"SELECT OBJECTPROPERTY(OBJECT_ID('" + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]'), 'TableHasIdentity');";
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection, SqlTransaction);
            return (int) sqlCommand.ExecuteScalar() == 1;
        }

        public void SetSqlConnection(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
        }

        public void SetSqlTransaction(SqlTransaction sqlTransaction)
        {
            SqlTransaction = sqlTransaction;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (SqlConnection != null)
                    SqlConnection.Close();
        }

        private SqlCommand CreateCommandSelect(object conditions)
        {
            if (conditions == null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }
            var conditionsList = new List<string>();
            var sqlParameters = new List<SqlParameter>();

            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var parameterProperties = conditions.GetType().GetProperties();

            foreach (var property in parameterProperties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }
                var value = property.GetValue(conditions, null);

                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                var typeProperty = typeProperties.First(x => x.Name == property.Name);
                var parameterName = '@' + typeProperty.Name;
                var columnAttribute = typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>()
                    .FirstOrDefault();

                var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;



                if (typeProperty.PropertyType == typeof(string))
                {
                    conditionsList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                  EntityUtils.GetTableName<TEntity>() +
                                  "].[" + columnName + "] LIKE '%' + " +
                                  parameterName +
                                  " + '%'");
                }
                else
                {
                    conditionsList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                  EntityUtils.GetTableName<TEntity>() +
                                  "].[" + columnName + "] = " + parameterName);
                }

                var sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName,
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

                sqlParameters.Add(sqlParameter);
            }


            var sqlQuery = @"SELECT " + EntityUtils.JoinColumns<TEntity>() + " FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (conditionsList.Count > 0)
                sqlQuery += " WHERE " + string.Join(" AND ", conditionsList);

            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);

            if (sqlParameters.Count > 0)
                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

            return sqlCommand;
        }

        private SqlCommand CreateCommandSelectById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var type = typeof(TEntity);
            var properties = type.GetProperties();
            string propertyName = null;
            Attribute keyAttribute = null;
            string columnName = null;
            foreach (var property in properties)
            {
                propertyName = '@' + property.Name;
                keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null)
                {
                    var columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : property.Name;
                    break;
                }
            }

            if (keyAttribute == null)
            {
                throw new InvalidOperationException("The entity has not established a KeyAttribute.");
            }
            
            var sqlQuery = @"SELECT " + EntityUtils.JoinColumns<TEntity>() + " FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] WHERE " + EntityUtils.GetSchema<TEntity>() + ".[" + EntityUtils.GetTableName<TEntity>() +
                           "].[" + columnName + "] = " + propertyName;

            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);
            sqlCommand.Parameters.AddWithValue(propertyName, id);
            return sqlCommand;
        }

        private SqlCommand CreateCommandInsert(object parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var columnList = new List<string>();
            var sqlParameterList = new List<SqlParameter>();

            var parameterProperties = parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var parameterProperty in parameterProperties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(parameterProperty.Name))
                {
                    continue;
                }

                var typeProperty = typeProperties.First(x => x.Name == parameterProperty.Name);

                var keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null && IsIdentityInsert())
                {
                    continue;
                }

                var value = parameterProperty.GetValue(parameters, null);
                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                var parameterName = '@' + typeProperty.Name;
                var columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;


                columnList.Add("[" + columnName + "]");

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName
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
            
            var sqlQuery = @"INSERT INTO " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] (" +
                           string.Join(", ", columnList) + ") VALUES(" +
                           string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                           "); SELECT CAST(SCOPE_IDENTITY() AS int);";
            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);

            if (sqlParameterList.Count > 0)
                sqlCommand.Parameters.AddRange(sqlParameterList.ToArray());

            return sqlCommand;
        }

        private SqlCommand CreateCommandUpdate(object parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var parametersList = new List<string>();
            var conditionsList = new List<string>();
            var sqlParameterList = new List<SqlParameter>();
            var parameterProperties = parameters.GetType().GetProperties();

            foreach (var property in parameterProperties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }

                var value = property.GetValue(parameters, null);

                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                var typeProperty = typeProperties.First(x => x.Name == property.Name);

                var parameterName = '@' + typeProperty.Name;
                var columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                var keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));

                if (keyAttribute != null)
                    conditionsList.Add("[" + columnName + "] = " + parameterName);
                else
                    parametersList.Add("[" + columnName + "] = " + parameterName);

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName
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


            if (parametersList.Count == 0)
            {
                throw new InvalidOperationException("Parameters list is empty.");
            }


            if (conditionsList.Count == 0)
            {
                throw new InvalidOperationException("Conditions list is empty.");
            }

            var sqlQuery = @"UPDATE " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] SET " +
                           string.Join(", ", parametersList) + " WHERE " + string.Join(" AND ", conditionsList);
            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);
            if (sqlParameterList.Count > 0)
                sqlCommand.Parameters.AddRange(sqlParameterList.ToArray());
            return sqlCommand;
        }

        private SqlCommand CreateCommandUpdate(object parameters, object conditions)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var parametersList = new List<string>();
            var conditionsList = new List<string>();
            var sqlParameterList = new List<SqlParameter>();
            var parameterCounter = 1;
            var parameterProperties = parameters.GetType().GetProperties();
            foreach (var property in parameterProperties)
            {
                if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                {
                    continue;
                }
                var typeProperty = typeProperties.First(x => x.Name == property.Name);
                var parameterName = '@' + typeProperty.Name;

                var columnAttribute =
                    typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                var value = property.GetValue(parameters, null);


                parametersList.Add("[" + columnName + "] = " + parameterName);

                SqlParameter sqlParameter = new SqlParameter
                {
                    ParameterName = parameterName
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

            if (conditions != null)
            {
                var conditionProperties = conditions.GetType().GetProperties();
                foreach (var property in conditionProperties)
                {
                    if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                    {
                        continue;
                    }

                    var typeProperty = typeProperties.First(x => x.Name == property.Name);
                    var value = property.GetValue(conditions, null);

                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    var columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                    var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                    string parameterName = '@' + typeProperty.Name;
                    if (sqlParameterList.Select(x => x.ParameterName).Contains(parameterName))
                    {
                        parameterName += "_" + parameterCounter;
                        parameterCounter++;
                    }
                    conditionsList.Add("[" + columnName + "] = " + parameterName);

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameterName
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

            if (parametersList.Count == 0)
            {
                throw new InvalidOperationException("Parameters list is empty.");
            }

            var sqlQuery = @"UPDATE " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] SET " +
                           string.Join(", ", parametersList);

            if (conditionsList.Count > 0)
            {
                sqlQuery += " WHERE " + string.Join(" AND ", conditionsList);
            }

            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);
            if (sqlParameterList.Count > 0)
                sqlCommand.Parameters.AddRange(sqlParameterList.ToArray());
            return sqlCommand;
        }

        private SqlCommand CreateDelete(object conditions)
        {
            var conditionsList = new List<string>();
            var sqlParameterList = new List<SqlParameter>();
            if (conditions != null)
            {
                var type = typeof(TEntity);
                var typeProperties = type.GetProperties();
                var properties = conditions.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (!typeProperties.Select(x => x.Name).Contains(property.Name))
                    {
                        continue;
                    }

                    var typeProperty = typeProperties.First(x => x.Name == property.Name);
                    var value = property.GetValue(conditions, null);

                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    var parameterName = '@' + typeProperty.Name;

                    var columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                    var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                    conditionsList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                  EntityUtils.GetTableName<TEntity>() +
                                  "].[" + columnName + "] = " + parameterName);

                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameterName
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

            var sqlQuery = @"DELETE FROM " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (conditionsList.Count > 0)
                sqlQuery += " WHERE " + string.Join(" AND ", conditionsList);

            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);
            if(sqlParameterList.Count > 0)
                sqlCommand.Parameters.AddRange(sqlParameterList.ToArray());
            return sqlCommand;
        }

        private SqlCommand CreateDeleteById(object id)
        {
            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            string where = null;
            string propertyName = null;
            if (id != null)
            {
                foreach (var typeProperty in typeProperties)
                {              
                    var keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                    if (keyAttribute == null) continue;
                    propertyName = '@' + typeProperty.Name;
                    var columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;
                    where = EntityUtils.GetSchema<TEntity>() + ".[" + EntityUtils.GetTableName<TEntity>() +
                            "].[" + columnName + "] = " + propertyName;
                    break;
                }
            }
            var sqlQuery = @"DELETE FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";
            if (where != null)
            {
                sqlQuery += " WHERE " + where;
            }

            var sqlCommand = SqlTransaction != null
                ? new SqlCommand(sqlQuery, SqlConnection, SqlTransaction)
                : new SqlCommand(sqlQuery, SqlConnection);

            if (propertyName != null)
            {
                sqlCommand.Parameters.AddWithValue(propertyName, id);
            }

            return sqlCommand;
        }
    }
}