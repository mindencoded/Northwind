using S3K.RealTimeOnline.DataAccess.Tools;
using S3K.RealTimeOnline.Domain.Entities;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected SqlConnection SqlConnection;
        protected SqlTransaction SqlTransaction;

        public Repository(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
        }

        public Repository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            SqlConnection = sqlConnection;
            SqlTransaction = sqlTransaction;
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            unitOfWork.Register(this);
        }

        public virtual IEnumerable<TEntity> SelectAll()
        {
            return SelectAll(null);
        }

        public virtual IEnumerable<TEntity> SelectAll(TEntity entity)
        {
            IList<TEntity> results;
            using (var sqlCommand = CreateSelectAllSqlCommand(entity))
            {
                if (SqlTransaction != null)
                {
                    sqlCommand.Transaction = SqlTransaction;
                }
                using (var sqlDataReader = sqlCommand.ExecuteReader())
                {
                    results = new List<TEntity>();
                    while (sqlDataReader.Read())
                    {
                        results.Add(sqlDataReader.ConvertToEntity<TEntity>());
                    }
                }
            }
            return results;
        }

        public virtual TEntity SelectById(TEntity entity)
        {
            TEntity result = null;
            using (var sqlCommand = CreateSelectByIdSqlCommand(entity))
            {
                if (SqlTransaction != null)
                {
                    sqlCommand.Transaction = SqlTransaction;
                }
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

        public virtual TEntity SelectById(object id)
        {
            TEntity result = null;
            using (var sqlCommand = CreateSelectByIdSqlCommand(id))
            {
                if (SqlTransaction != null)
                {
                    sqlCommand.Transaction = SqlTransaction;
                }
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
            var sqlCommand = CreateInsertSqlCommand(entity);
            if (SqlTransaction != null)
            {
                sqlCommand.Transaction = SqlTransaction;
            }
            int.TryParse(sqlCommand.ExecuteScalar().ToString(), out result);
            return result;
        }

        public virtual int Update(TEntity entity)
        {
            var sqlCommand = CreateUpdateSqlCommand(entity);
            if (SqlTransaction != null)
            {
                sqlCommand.Transaction = SqlTransaction;
            }
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Delete()
        {
            return Delete(null);
        }

        public virtual int Delete(TEntity entity)
        {
            var sqlCommand = CreateDeleteSqlCommand(entity);
            if (SqlTransaction != null)
            {
                sqlCommand.Transaction = SqlTransaction;
            }
            return sqlCommand.ExecuteNonQuery();
        }

        public virtual int Delete(object id)
        {
            var sqlCommand = CreateDeleteByIdSqlCommand(id);
            if (SqlTransaction != null)
            {
                sqlCommand.Transaction = SqlTransaction;
            }
            return sqlCommand.ExecuteNonQuery();
        }

        public bool IsOpenConnection()
        {
            return SqlConnection != null && SqlConnection.State == ConnectionState.Open;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (SqlConnection != null)
                {
                    SqlConnection.Close();
                }
            }
        }

        private SqlCommand CreateInsertSqlCommand(TEntity instance)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var columnList = new List<string>();
            var parameterList = new List<string>();
            var isIdentityInsert = IsIdentityInsert();
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
                        if (keyAttribute != null && isIdentityInsert)
                        {
                            continue;
                        }
                        var value = property.GetValue(instance, null);
                        if (value != null)
                        {
                            columnList.Add("[" + columnAttribute.Name + "]");
                            parameterList.Add(propertyName);
                            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                            {
                                var dateValue = (DateTime?)value;
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
            var sqlQuery = @"INSERT INTO " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "] (" +
                           string.Join(", ", columnList) + ") VALUES(" + string.Join(", ", parameterList) +
                           "); select SCOPE_IDENTITY()";
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            foreach (var keyValuePair in parameterDictionary)
            {
                sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            }
            return sqlCommand;
        }

        private SqlCommand CreateUpdateSqlCommand(TEntity instance)
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


                        var value = property.GetValue(instance, null);
                        if (value != null)
                        {
                            if (keyAttribute != null)
                            {
                                whereList.Add("[" + columnAttribute.Name + "] = " + propertyName);
                            }
                            else
                            {
                                setList.Add("[" + columnAttribute.Name + "] = " + propertyName);
                            }

                            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                            {
                                var dateValue = (DateTime?)value;
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
            {
                sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            }
            return sqlCommand;
        }

        private SqlCommand CreateSelectByIdSqlCommand(TEntity instance)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();

            foreach (var property in properties)
            {
                var propertyName = '@' + property.Name;
                try
                {
                    var columnAttribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                    if (columnAttribute != null && keyAttribute != null)
                    {
                        var value = property.GetValue(instance, null);
                        if (value != null)
                        {
                            whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" + EntityUtils.GetTableName<TEntity>() +
                                          "].[" + columnAttribute.Name + "] = " + propertyName);
                            parameterDictionary.Add(propertyName, value);
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
                           EntityUtils.GetTableName<TEntity>() + "] WHERE " + string.Join(" AND ", whereList);
            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);
            foreach (var keyValuePair in parameterDictionary)
            {
                sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
            }
            return sqlCommand;
        }

        private SqlCommand CreateSelectByIdSqlCommand(object id)
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

        private SqlCommand CreateSelectAllSqlCommand(TEntity instance)
        {
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();

            if (instance != null)
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
                            var value = property.GetValue(instance, null);
                            if (value != null)
                            {
                                var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));

                                if (keyAttribute != null)
                                {
                                    int key;
                                    if (int.TryParse(value.ToString(), out key))
                                    {
                                        if (key == 0)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                if (property.PropertyType == typeof(string))
                                {
                                    whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                                  EntityUtils.GetTableName<TEntity>() +
                                                  "].[" + columnAttribute.Name + "] LIKE '%' + " + propertyName + " + '%'");
                                }
                                else
                                {
                                    whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                                  EntityUtils.GetTableName<TEntity>() +
                                                  "].[" + columnAttribute.Name + "] = " + propertyName);
                                }
                              

                                if (property.PropertyType == typeof(DateTime) ||  property.PropertyType == typeof(DateTime?))
                                {
                                    var dateValue = (DateTime?)value;
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
            }

            var sqlQuery = @"SELECT " + EntityUtils.JoinColumns<TEntity>() + " FROM " +
                           EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (whereList.Count > 0)
            {
                sqlQuery += " WHERE " + string.Join(" AND ", whereList);
            }

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            if (parameterDictionary.Count > 0)
            {
                foreach (var keyValuePair in parameterDictionary)
                {
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return sqlCommand;
        }

        private SqlCommand CreateDeleteSqlCommand(TEntity instance)
        {
            var whereList = new List<string>();
            IDictionary<string, object> parameterDictionary = new ConcurrentDictionary<string, object>();

            if (instance != null)
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
                            var value = property.GetValue(instance, null);
                            if (value != null)
                            {
                                var keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));

                                if (keyAttribute != null)
                                {
                                    int key;
                                    if (int.TryParse(value.ToString(), out key))
                                    {
                                        if (key == 0)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                whereList.Add(EntityUtils.GetSchema<TEntity>() + ".[" +
                                              EntityUtils.GetTableName<TEntity>() +
                                              "].[" + columnAttribute.Name + "] = " + propertyName);
                                if (property.PropertyType == typeof(DateTime) ||
                                    property.PropertyType == typeof(DateTime?))
                                {
                                    var dateValue = (DateTime?)value;
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
            }

            var sqlQuery = @"DELETE FROM " + EntityUtils.GetSchema<TEntity>() + ".[" +
                           EntityUtils.GetTableName<TEntity>() + "]";

            if (whereList.Count > 0)
            {
                sqlQuery += " WHERE " + string.Join(" AND ", whereList);
            }

            var sqlCommand = new SqlCommand(sqlQuery, SqlConnection);

            if (parameterDictionary.Count > 0)
            {
                foreach (var keyValuePair in parameterDictionary)
                {
                    sqlCommand.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return sqlCommand;
        }

        private SqlCommand CreateDeleteByIdSqlCommand(object id)
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

        public bool IsIdentityInsert()
        {
            using (var sqlCommand = SqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = @"SELECT OBJECTPROPERTY(OBJECT_ID('" + EntityUtils.GetSchema<TEntity>() + ".[" +
                                         EntityUtils.GetTableName<TEntity>() + "]'), 'TableHasIdentity');";
                return (int)sqlCommand.ExecuteScalar() == 1;
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
    }
}