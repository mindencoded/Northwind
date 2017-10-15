using System;
using System.Collections.Concurrent;
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
using Serilog;

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

        public virtual IEnumerable<TEntity> Select(TEntity entity)
        {
            IList<TEntity> results;
            using (var sqlCommand = CreateSelect(entity))
            {
                if (SqlTransaction != null)
                    sqlCommand.Transaction = SqlTransaction;
                using (var sqlDataReader = sqlCommand.ExecuteReader())
                {
                    results = new List<TEntity>();
                    while (sqlDataReader.Read())
                        results.Add(sqlDataReader.ConvertToEntity<TEntity>());
                }
            }
            return results;
        }

        public IEnumerable<TEntity> Select(object instance)
        {
            IList<TEntity> results;
            using (var sqlCommand = CreateSelect(instance))
            {
                if (SqlTransaction != null)
                    sqlCommand.Transaction = SqlTransaction;
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
            using (var sqlCommand = CreateSelectById(id))
            {
                if (SqlTransaction != null)
                    sqlCommand.Transaction = SqlTransaction;
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

        public virtual int Insert(TEntity entity)
        {
            int result;
            var sqlCommand = CreateInsert(entity);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            //int.TryParse(sqlCommand.ExecuteScalar().ToString(), out result);
            result = (int)sqlCommand.ExecuteScalar();
            return result;
        }

        public virtual int Insert(object instance)
        {
            int result;
            var sqlCommand = CreateInsert(instance);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            //int.TryParse(sqlCommand.ExecuteScalar().ToString(), out result);
            result = (int) sqlCommand.ExecuteScalar();
            return result;
        }

        public virtual int Update(TEntity entity)
        {
            var sqlCommand = CreateUpdate(entity);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Update(object instance)
        {
            var sqlCommand = CreateUpdate(instance);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            return sqlCommand.ExecuteNonQuery();
        }

        public int Update(object instance, object conditions)
        {
            var sqlCommand = CreateUpdate(instance, conditions);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Delete(TEntity entity)
        {
            var sqlCommand = CreateDelete(entity);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Delete(object instance)
        {
            var sqlCommand = CreateDelete(instance);
            if (SqlTransaction != null)
                sqlCommand.Transaction = SqlTransaction;
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
            using (var sqlCommand = SqlConnection.CreateCommand())
            {
                sqlCommand.CommandText =
                    @"SELECT OBJECTPROPERTY(OBJECT_ID('" + EntityUtils.GetSchema<TEntity>() + ".[" +
                    EntityUtils.GetTableName<TEntity>() + "]'), 'TableHasIdentity');";
                return (int) sqlCommand.ExecuteScalar() == 1;
            }
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

        private SqlCommand CreateSelect(TEntity entity)
        {
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();

            if (entity != null)
            {
                var type = typeof(TEntity);
                var properties = type.GetProperties();

                try
                {
                    foreach (var property in properties)
                    {
                        var propertyName = '@' + property.Name;
                        var columnAttribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        if (columnAttribute != null)
                        {
                            var value = property.GetValue(entity, null);

                            if (value == null)
                            {
                                continue;
                            }

                            if (property.GetCustomAttribute(typeof(KeyAttribute)) != null)
                            {
                                int id;
                                if (int.TryParse(value.ToString(), out id))
                                    if (id == 0)
                                        continue;

                            }

                            if (property.PropertyType == typeof(string))
                                whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                              EntityUtils.GetTableName<TEntity>() +
                                              "].[" + columnAttribute.Name + "] LIKE '%' + " + propertyName +
                                              " + '%'");
                            else
                                whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                              EntityUtils.GetTableName<TEntity>() +
                                              "].[" + columnAttribute.Name + "] = " + propertyName);


                            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                            {
                                var dateValue = (DateTime?) value;
                                parameterDictionary.Add(propertyName,
                                    dateValue.Value.ToString(DbManager.DateFormat));
                            }
                            else
                            {
                                parameterDictionary.Add(propertyName, value);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            var sqlQuery = @"SELECT " + EntityUtils.JoinColumns<TEntity>() + " FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (whereList.Count > 0)
                sqlQuery += " WHERE " + string.Join(" AND ", whereList);

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            if (parameterDictionary.Count > 0)
                foreach (var keyValuePair in parameterDictionary)
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateSelect(object instance)
        {
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();
            if (instance != null)
            {
                var type = typeof(TEntity);
                var typeProperties = type.GetProperties();
                try
                {
                    foreach (var typeProperty in typeProperties)
                    {
                        foreach (var instanceProperty in instance.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            if (!typeProperty.Name.Equals(instanceProperty.Name))
                            {
                                continue;
                            }

                            var parameterName = '@' + typeProperty.Name;
                            var columnAttribute = typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>()
                                .FirstOrDefault();

                            var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                            var value = instanceProperty.GetValue(instance, null);

                            if (value != null && typeProperty.PropertyType == typeof(string))
                            {
                                whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                              EntityUtils.GetTableName<TEntity>() +
                                              "].[" + columnName + "] LIKE '%' + " +
                                              parameterName +
                                              " + '%'");
                            }
                            else
                            {
                                whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                              EntityUtils.GetTableName<TEntity>() +
                                              "].[" + columnName + "] = " + parameterName);
                            }


                            if (value != null && (typeProperty.PropertyType == typeof(DateTime) ||
                                                  typeProperty.PropertyType == typeof(DateTime?)))
                            {
                                var dateValue = (DateTime?) value;
                                parameterDictionary.Add(parameterName, dateValue.Value.ToString(DbManager.DateFormat));
                            }
                            else
                            {
                                parameterDictionary.Add(parameterName, value ?? DBNull.Value);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            var sqlQuery = @"SELECT " + EntityUtils.JoinColumns<TEntity>() + " FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (whereList.Count > 0)
                sqlQuery += " WHERE " + string.Join(" AND ", whereList);

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            if (parameterDictionary.Count > 0)
                foreach (var keyValuePair in parameterDictionary)
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateSelectById(object id)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var where = "";
            var propertyName = "";
            foreach (var property in properties)
            {
                propertyName = '@' + property.Name;
                try
                {
                    var columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                    if (columnAttribute != null && keyAttribute != null)
                    {
                        where = EntityUtils.GetSchema<TEntity>() + ".[" + EntityUtils.GetTableName<TEntity>() +
                                "].[" + columnAttribute.Name + "] = " + propertyName;
                        break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            var sqlQuery = @"SELECT " + EntityUtils.JoinColumns<TEntity>() + " FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] WHERE " + where;

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            sqlCommand.Parameters.AddWithValue(propertyName, id);
            return sqlCommand;
        }

        private SqlCommand CreateInsert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var columnList = new List<string>();
            var sqlParameterList = new List<SqlParameter>();
            try
            {
                foreach (var property in properties)
                {
                    var parameterName = '@' + property.Name;
                    var value = property.GetValue(entity, null);
                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                    if (keyAttribute != null && IsIdentityInsert())
                    {
                        continue;
                    }

                    var columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    string columnName = columnAttribute != null ? columnAttribute.Name : property.Name;


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
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            var sqlQuery = @"INSERT INTO " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] (" +
                           string.Join(", ", columnList) + ") VALUES (" +
                           string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                           "); SELECT CAST(SCOPE_IDENTITY() AS int);";
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            sqlCommand.Parameters.AddRange(sqlParameterList.ToArray());
            return sqlCommand;
        }

        private SqlCommand CreateInsert(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var columnList = new List<string>();
            var sqlParameterList = new List<SqlParameter>();
            try
            {
                var instanceProperties = type.GetProperties();
                foreach (var instanceProperty in instanceProperties)
                {
                    if (typeProperties.Select(x => x.Name).Contains(instanceProperty.Name))
                    {
                        continue;
                    }

                    var typeProperty = typeProperties.First(x => x.Name == instanceProperty.Name);

                    var value = typeProperty.GetValue(instance, null);
                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    var parameterName = '@' + typeProperty.Name;
                    var columnAttribute = typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
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

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            var sqlQuery = @"INSERT INTO " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] (" +
                           string.Join(", ", columnList) + ") VALUES(" +
                           string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                           "); SELECT CAST(SCOPE_IDENTITY() AS int);";
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            sqlCommand.Parameters.AddRange(sqlParameterList.ToArray());

            return sqlCommand;
        }

        private SqlCommand CreateUpdate(TEntity entity)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var setList = new List<string>();
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();

            foreach (var property in properties)
            {
                var propertyName = '@' + property.Name;
                try
                {
                    var columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    if (columnAttribute != null)
                    {
                        var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));


                        var value = property.GetValue(entity, null);
                        if (value != null)
                        {
                            if (keyAttribute != null)
                                whereList.Add("[" + columnAttribute.Name + "] = " + propertyName);
                            else
                                setList.Add("[" + columnAttribute.Name + "] = " + propertyName);

                            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                            {
                                var dateValue = (DateTime?) value;
                                parameterDictionary.Add(propertyName, dateValue.Value.ToString(DbManager.DateFormat));
                            }
                            else
                            {
                                parameterDictionary.Add(propertyName, value);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
            var sqlQuery = @"UPDATE " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] SET " +
                           string.Join(", ", setList) + " WHERE " + string.Join(" AND ", whereList);
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            foreach (var keyValuePair in parameterDictionary)
                sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateUpdate(object instance)
        {
            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var setList = new List<string>();
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();
            try
            {
                foreach (var typeProperty in typeProperties)
                {
                    foreach (var instanceProperty in instance.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (!typeProperty.Name.Equals(instanceProperty.Name))
                        {
                            continue;
                        }
                        var parameterName = '@' + typeProperty.Name;
                        var columnAttribute =
                            typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                        var keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));

                        var value = typeProperty.GetValue(instance, null);

                        if (keyAttribute != null)
                            whereList.Add("[" + columnName + "] = " + parameterName);
                        else
                            setList.Add("[" + columnName + "] = " + parameterName);

                        if (value != null && (typeProperty.PropertyType == typeof(DateTime) ||
                                              typeProperty.PropertyType == typeof(DateTime?)))
                        {
                            var dateValue = (DateTime?) value;
                            parameterDictionary.Add(parameterName, dateValue.Value.ToString(DbManager.DateFormat));
                        }
                        else
                        {
                            parameterDictionary.Add(parameterName, value ?? DBNull.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            var sqlQuery = @"UPDATE " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] SET " +
                           string.Join(", ", setList) + " WHERE " + string.Join(" AND ", whereList);
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            foreach (var keyValuePair in parameterDictionary)
                sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateUpdate(object instance, object conditions)
        {
            var type = typeof(TEntity);
            var typeProperties = type.GetProperties();
            var setList = new List<string>();
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();
            try
            {
                foreach (var typeProperty in typeProperties)
                {
                    var parameterName = '@' + typeProperty.Name;
                  
                    foreach (var instanceProperty in instance.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (!typeProperty.Name.Equals(instanceProperty.Name))
                        {
                            continue;
                        }

                        

                        var columnAttribute =
                            typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                        var value = typeProperty.GetValue(instance, null);

                       
                        setList.Add("[" + columnName + "] = " + parameterName);

                        if (value != null && (typeProperty.PropertyType == typeof(DateTime) ||
                                              typeProperty.PropertyType == typeof(DateTime?)))
                        {
                            var dateValue = (DateTime?)value;
                            parameterDictionary.Add(parameterName, dateValue.Value.ToString(DbManager.DateFormat));
                        }
                        else
                        {
                            parameterDictionary.Add(parameterName, value ?? DBNull.Value);
                        }
                    }

                    foreach (var conditionProperty in conditions.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (!typeProperty.Name.Equals(conditionProperty.Name))
                        {
                            continue;
                        }

                        var columnAttribute =
                            typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;


                        var value = typeProperty.GetValue(instance, null);


                        whereList.Add("[" + columnName + "] = " + parameterName);

                        if (value != null && (typeProperty.PropertyType == typeof(DateTime) ||
                                              typeProperty.PropertyType == typeof(DateTime?)))
                        {
                            var dateValue = (DateTime?) value;
                            parameterDictionary.Add(parameterName, dateValue.Value.ToString(DbManager.DateFormat));
                        }
                        else
                        {
                            parameterDictionary.Add(parameterName, value ?? DBNull.Value);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            var sqlQuery = @"UPDATE " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] SET " +
                           string.Join(", ", setList) + " WHERE " + string.Join(" AND ", whereList);
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            foreach (var keyValuePair in parameterDictionary)
                sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateDelete(TEntity entity)
        {
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();
            if (entity != null)
            {
                var type = typeof(TEntity);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var propertyName = '@' + property.Name;
                    try
                    {
                        var columnAttribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        if (columnAttribute != null)
                        {
                            var value = property.GetValue(entity, null);
                            if (value != null)
                            {
                                var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));

                                if (keyAttribute != null)
                                {
                                    int key;
                                    if (int.TryParse(value.ToString(), out key))
                                        if (key == 0)
                                            continue;
                                }

                                whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                              EntityUtils.GetTableName<TEntity>() +
                                              "].[" + columnAttribute.Name + "] = " + propertyName);
                                if (property.PropertyType == typeof(DateTime) ||
                                    property.PropertyType == typeof(DateTime?))
                                {
                                    var dateValue = (DateTime?) value;
                                    parameterDictionary.Add(propertyName,
                                        dateValue.Value.ToString(DbManager.DateFormat));
                                }
                                else
                                {
                                    parameterDictionary.Add(propertyName, value);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                    }
                }
            }

            var sqlQuery = @"DELETE FROM " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (whereList.Count > 0)
                sqlQuery += " WHERE " + string.Join(" AND ", whereList);

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            if (parameterDictionary.Count > 0)
                foreach (var keyValuePair in parameterDictionary)
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateDelete(object instance)
        {
            var whereList = new List<string>();
            var parameterDictionary = new Dictionary<string, object>();
            if (instance != null)
            {
                var type = typeof(TEntity);
                var typeProperties = type.GetProperties();
                try
                {
                    foreach (var typeProperty in typeProperties)
                    {
                        foreach (var instanceProperty in instance.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            if (!typeProperty.Name.Equals(instanceProperty.Name))
                            {
                                continue;
                            }

                            var parameterName = '@' + typeProperty.Name;
                            var columnAttribute =
                                typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                            var columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                            var value = typeProperty.GetValue(instance, null);

                            whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                          EntityUtils.GetTableName<TEntity>() +
                                          "].[" + columnName + "] = " + parameterName);
                            if (value != null && (typeProperty.PropertyType == typeof(DateTime) ||
                                                  typeProperty.PropertyType == typeof(DateTime?)))
                            {
                                var dateValue = (DateTime?) value;
                                parameterDictionary.Add(parameterName,
                                    dateValue.Value.ToString(DbManager.DateFormat));
                            }
                            else
                            {
                                parameterDictionary.Add(parameterName, value ?? DBNull.Value);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }

            }

            var sqlQuery = @"DELETE FROM " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (whereList.Count > 0)
                sqlQuery += " WHERE " + string.Join(" AND ", whereList);

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            if (parameterDictionary.Count > 0)
                foreach (var keyValuePair in parameterDictionary)
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            return sqlCommand;
        }

        private SqlCommand CreateDeleteById(object id)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var where = "";
            var propertyName = "";
            foreach (var property in properties)
            {
                propertyName = '@' + property.Name;
                try
                {
                    var columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                    if (columnAttribute != null && keyAttribute != null)
                    {
                        where = EntityUtils.GetSchema<TEntity>() + ".[" + EntityUtils.GetTableName<TEntity>() +
                                "].[" + columnAttribute.Name + "] = " + propertyName;
                        break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            var sqlQuery = @"DELETE FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] WHERE " + where;

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            sqlCommand.Parameters.AddWithValue(propertyName, id.ToString());
            return sqlCommand;
        }
    }
}