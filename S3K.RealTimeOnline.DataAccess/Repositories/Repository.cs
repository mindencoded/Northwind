﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using S3K.RealTimeOnline.Commons;
using S3K.RealTimeOnline.DataAccess.Tools;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;

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
            _columnAttributes = typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(SqlConnection connection, bool ignoreNulls)
        {
            Connection = connection;
            IgnoreNulls = ignoreNulls;
            _columnAttributes = typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(SqlConnection connection, SqlTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
            IgnoreNulls = true;
            _columnAttributes = typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls)
        {
            Connection = connection;
            Transaction = transaction;
            IgnoreNulls = ignoreNulls;
            _columnAttributes = typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            unitOfWork.Register(this);
            _columnAttributes = typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public Repository(IUnitOfWork unitOfWork, bool ignoreNulls)
        {
            unitOfWork.Register(this);
            IgnoreNulls = ignoreNulls;
            _columnAttributes = typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault());
        }

        public IEnumerable<dynamic> Select(IEnumerable<string> columns, string orderBy = null, int? page = null,
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

        public virtual IEnumerable<T> Select(object conditions, string orderBy = null, int? page = null,
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

        public virtual IEnumerable<dynamic> Select(IEnumerable<string> columns, object conditions,
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

        public virtual IEnumerable<dynamic> Select(IEnumerable<string> columns, IDictionary<string, object> conditions,
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

        public virtual int Insert(IDictionary<string, object> parameters)
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

        public virtual int Update(IDictionary<string, object> parameters)
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

        public virtual int Update(IDictionary<string, object> parameters, IDictionary<string, object> conditions)
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
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            using (command)
            {
                return (int) command.ExecuteScalar() == 1;
            }

        }

        public void SetSqlConnection(SqlConnection sqlConnection)
        {
            Connection = sqlConnection;
        }

        public void SetSqlTransaction(SqlTransaction sqlTransaction)
        {
            Transaction = sqlTransaction;
        }

        public object IdentCurrent()
        {
            string query = "SELECT IDENT_CURRENT('" + EntityUtils.GetTableName<T>() + "');";
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            using (command)
            {
                return command.ExecuteScalar();
            }
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

        private string CreateSelectStatement()
        {
            return @"SELECT " + EntityUtils.JoinColumns<T>() + " FROM " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "]";
        }

        private string CreateSelectStatement(
            IEnumerable<string> columns,
            out IList<SqlParameter> sqlParameterList,
            string orderBy = null,
            int? page = null, int?
                pageSize = null)
        {
            string columnQuery;
            if (columns != null)
            {
                string[] enumerable = columns as string[];
                if (page != null && pageSize != null)
                {

                    columnQuery = EntityUtils.JoinColumns<T>(enumerable, true, true);

                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>(enumerable, true);
                }
            }
            else
            {
                if (page != null && pageSize != null)
                {

                    columnQuery = EntityUtils.JoinColumns<T>(true);

                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>();
                }
            }

            sqlParameterList = new List<SqlParameter>();



            string query = @"SELECT " + columnQuery + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]";



            if (page != null && pageSize != null)
            {
                sqlParameterList.Add(new SqlParameter
                {
                    ParameterName = "@Page",
                    SqlDbType = SqlDbType.Int,
                    Value = page
                });

                sqlParameterList.Add(new SqlParameter
                {
                    ParameterName = "@PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = pageSize
                });

                if (columns != null)
                {
                    string[] enumerable = columns as string[];
                    columnQuery = EntityUtils.SimpleJoinColumns<T>(enumerable, true);
                }
                else
                {
                    columnQuery = EntityUtils.SimpleJoinColumns<T>();
                }

                query = @"WITH PageNumbers AS (" + query + ") SELECT " + columnQuery +
                        " FROM  PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
                //query = @"SELECT " + columnsStr + " FROM (" + query + ") AS PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query += " ORDER BY " + orderBy;
            }

            return query;
        }

        private string CreateSelectStatement(
            IEnumerable<string> columns,
            object conditions,
            out IList<SqlParameter> sqlParameterList,
            string orderBy = null,
            int? page = null, int?
                pageSize = null)
        {
            string columnQuery;
            if (columns != null)
            {
                string[] enumerable = columns as string[];
                if (page != null && pageSize != null)
                {

                    columnQuery = EntityUtils.JoinColumns<T>(enumerable, true, true);

                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>(enumerable, true);
                }
            }
            else
            {
                if (page != null && pageSize != null)
                {

                    columnQuery = EntityUtils.JoinColumns<T>(true);

                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>();
                }
            }

            sqlParameterList = new List<SqlParameter>();
            IList<string> conditionList = new List<string>();
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

                    object value = property.GetValue(conditions, null);

                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }

                    PropertyInfo propertyInfo = typeProperties.First(x => x.Name == property.Name);
                    Type propertyType = propertyInfo.PropertyType;
                    ColumnAttribute columnAttribute = propertyInfo.GetCustomAttributes(false).OfType<ColumnAttribute>()
                        .FirstOrDefault();
                    string propertyName = propertyInfo.Name;
                    string columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                    IDictionary<string, object> parameters = new Dictionary<string, object>();
                    if (value != null && value is string)
                    {
                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" +
                                          EntityUtils.GetTableName<T>() +
                                          "].[" + columnName + "] LIKE '%' + @" + propertyName +
                                          " + '%'");

                        parameters.Add("@" + propertyName, value);
                    }
                    else if (value != null && value.GetType().IsGenericType &&
                             value.GetType().GetGenericTypeDefinition() == typeof(Tuple<,>))
                    {

                        conditionList.Add("(" + EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                          "].[" + columnName + "] BETWEEN @" + propertyName + "1 AND @" + propertyName +
                                          "2)");
                        foreach (FieldInfo field in value.GetType().GetFields())
                        {

                            parameters.Add("@" + propertyName + "1", field.GetValue(value));
                            parameters.Add("@" + propertyName + "2", field.GetValue(value));
                        }

                    }
                    else if (value != null && value.GetType().IsArray)
                    {
                        object[] items = (object[]) value;
                        IList<string> subConditionList = new List<string>();
                        if (typeof(string).IsAssignableFrom(value.GetType().GetElementType()))
                        {
                            for (int i = 0; i < items.Length; i++)
                            {
                                subConditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                                     "].[" + columnName + "] LIKE '%' + @" + propertyName + i +
                                                     " + '%'");
                                parameters.Add("@" + propertyName + i, items[i]);
                            }
                            conditionList.Add("(" + string.Join(" OR ", subConditionList) + ")");
                        }
                        else
                        {
                            for (int i = 0; i < items.Length; i++)
                            {
                                subConditionList.Add("@" + propertyName + i);
                                parameters.Add("@" + propertyName + i, items[i]);
                            }
                            conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                              "].[" + columnName + "] IN (" + string.Join(", ", subConditionList) +
                                              ")");
                        }
                    }
                    else
                    {
                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() + "].[" +
                                          columnName + "] = @" + propertyName);
                        parameters.Add("@" + propertyName, value);
                    }

                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        SqlParameter sqlParameter = new SqlParameter
                        {
                            ParameterName = parameter.Key,
                            SourceColumn = columnName,
                            SqlDbType = TypeConvertor.ToSqlDbType(propertyType),
                            Value = parameter.Value ?? DBNull.Value
                        };
                        sqlParameterList.Add(sqlParameter);
                    }
                }
            }


            string query = @"SELECT " + columnQuery + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]";

            if (conditionList.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditionList);
            }

            if (page != null && pageSize != null)
            {
                sqlParameterList.Add(new SqlParameter
                {
                    ParameterName = "@Page",
                    SqlDbType = SqlDbType.Int,
                    Value = page
                });

                sqlParameterList.Add(new SqlParameter
                {
                    ParameterName = "@PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = pageSize
                });

                if (columns != null)
                {
                    string[] enumerable = columns as string[];
                    columnQuery = EntityUtils.SimpleJoinColumns<T>(enumerable, true);
                }
                else
                {
                    columnQuery = EntityUtils.SimpleJoinColumns<T>();
                }

                query = @"WITH PageNumbers AS (" + query + ") SELECT " + columnQuery +
                        " FROM  PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
                //query = @"SELECT " + columnsStr + " FROM (" + query + ") AS PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query += " ORDER BY " + orderBy;
            }

            return query;
        }

        private string CreateSelectStatement(
            IEnumerable<string> columns,
            IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList,
            string orderBy = null,
            int? page = null, int?
                pageSize = null)
        {
            string columnQuery;
            if (columns != null)
            {
                string[] enumerable = columns as string[];
                if (page != null && pageSize != null)
                {

                    columnQuery = EntityUtils.JoinColumns<T>(enumerable, true, true);

                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>(enumerable, true);
                }
            }
            else
            {
                if (page != null && pageSize != null)
                {

                    columnQuery = EntityUtils.JoinColumns<T>(true);

                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>();
                }
            }

            sqlParameterList = new List<SqlParameter>();
            IList<string> conditionList = new List<string>();
            if (conditions != null)
            {
                Type type = typeof(T);
                PropertyInfo[] typeProperties = type.GetProperties();

                foreach (KeyValuePair<string, object> condition in conditions)
                {
                    object value = condition.Value;
                    if (value == null && IgnoreNulls)
                    {
                        continue;
                    }
                    string columnName = null;
                    string propertyName = condition.Key.TrimStart('@'); //entry.Key.Replace("@", "").Trim();

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

                    IDictionary<string, object> parameters = new Dictionary<string, object>();
                    if (value != null && value is string)
                    {
                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" +
                                          EntityUtils.GetTableName<T>() +
                                          "].[" + columnName + "] LIKE '%' + @" + propertyName +
                                          " + '%'");

                        parameters.Add("@" + propertyName, value);
                    }
                    else if (value != null && value.GetType().IsGenericType &&
                             value.GetType().GetGenericTypeDefinition() == typeof(Tuple<,>))
                    {

                        conditionList.Add("(" + EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                          "].[" + columnName + "] BETWEEN @" + propertyName + "1 AND @" + propertyName +
                                          "2)");
                        foreach (FieldInfo field in value.GetType().GetFields())
                        {

                            parameters.Add("@" + propertyName + "1", field.GetValue(value));
                            parameters.Add("@" + propertyName + "2", field.GetValue(value));
                        }

                    }
                    else if (value != null && value.GetType().IsArray)
                    {
                        object[] items = (object[]) value;
                        IList<string> subConditionList = new List<string>();
                        if (typeof(string).IsAssignableFrom(value.GetType().GetElementType()))
                        {
                            for (int i = 0; i < items.Length; i++)
                            {
                                subConditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                                     "].[" + columnName + "] LIKE '%' + @" + propertyName + i +
                                                     " + '%'");
                                parameters.Add("@" + propertyName + i, items[i]);
                            }
                            conditionList.Add("(" + string.Join(" OR ", subConditionList) + ")");
                        }
                        else
                        {
                            for (int i = 0; i < items.Length; i++)
                            {
                                subConditionList.Add("@" + propertyName + i);
                                parameters.Add("@" + propertyName + i, items[i]);
                            }
                            conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() +
                                              "].[" + columnName + "] IN (" + string.Join(", ", subConditionList) +
                                              ")");
                        }
                    }
                    else
                    {
                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableName<T>() + "].[" +
                                          columnName + "] = @" + propertyName);
                        parameters.Add("@" + propertyName, value);
                    }

                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        SqlParameter sqlParameter = new SqlParameter
                        {
                            ParameterName = parameter.Key,
                            SourceColumn = columnName,
                            SqlDbType = TypeConvertor.ToSqlDbType(value.GetType()),
                            Value = parameter.Value ?? DBNull.Value
                        };
                        sqlParameterList.Add(sqlParameter);
                    }
                }
            }


            string query = @"SELECT " + columnQuery + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "]";

            if (conditionList.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditionList);
            }

            if (page != null && pageSize != null)
            {
                sqlParameterList.Add(new SqlParameter
                {
                    ParameterName = "@Page",
                    SqlDbType = SqlDbType.Int,
                    Value = page
                });

                sqlParameterList.Add(new SqlParameter
                {
                    ParameterName = "@PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = pageSize
                });

                if (columns != null)
                {
                    string[] enumerable = columns as string[];
                    columnQuery = EntityUtils.SimpleJoinColumns<T>(enumerable, true);
                }
                else
                {
                    columnQuery = EntityUtils.SimpleJoinColumns<T>();
                }

                query = @"WITH PageNumbers AS (" + query + ") SELECT " + columnQuery +
                        " FROM  PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
                //query = @"SELECT " + columnsStr + " FROM (" + query + ") AS PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
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
                   string.Join(", ", columnList) + ") VALUES (" +
                   string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                   ");SELECT CAST(SCOPE_IDENTITY() AS int);";
        }

        private string CreateInsertStatement(object parameters, out IList<SqlParameter> sqlParameterList)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> columnNameList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
            PropertyInfo[] properties = parameters.GetType().GetProperties();

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

            return @"INSERT INTO " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] (" +
                   string.Join(", ", columnNameList) + ") VALUES(" +
                   string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                   "); SELECT CAST(SCOPE_IDENTITY() AS int);";
        }

        private string CreateInsertStatement(IDictionary<string, object> parameters,
            out IList<SqlParameter> sqlParameterList)
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            IList<string> columnNameList = new List<string>();
            sqlParameterList = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object value = parameter.Value;
                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                string propertyName = parameter.Key.TrimStart('@');
                string columnName = null;
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
                columnNameList.Add("[" + columnName + "]");
                string parameterName = "@" + propertyName;

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


            return @"INSERT INTO " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] (" +
                   string.Join(", ", columnNameList) + ") VALUES(" +
                   string.Join(", ", sqlParameterList.Select(x => x.ParameterName)) +
                   "); SELECT CAST(SCOPE_IDENTITY() AS int);";
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
                if (columnName.ToUpper().Equals("CREATED"))
                {
                    continue;
                }
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

        private string CreateUpdateStatement(object parameters, out IList<SqlParameter> sqlParameterList)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
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

                if (columnName.ToUpper().Equals("CREATED"))
                {
                    continue;
                }

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

            return @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] SET " +
                   string.Join(", ", parameterList) + " WHERE " + string.Join(" AND ", conditionList);
        }

        private string CreateUpdateStatement(IDictionary<string, object> parameters,
            out IList<SqlParameter> sqlParameterList)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object value = parameter.Value;
                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                string propertyName = parameter.Key.TrimStart('@');
                string columnName = null;
                Attribute keyAttribute = null;
                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                    keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                }
                else
                {
                    if (_columnAttributes.Select(x => x.Name).Contains(propertyName))
                    {
                        columnName = propertyName;
                        foreach (PropertyInfo propertyInfo in typeProperties)
                        {
                            ColumnAttribute columnAttribute = propertyInfo
                                .GetCustomAttributes(typeof(ColumnAttribute), false)
                                .Cast<ColumnAttribute>().FirstOrDefault();
                            if (columnAttribute != null)
                            {
                                keyAttribute = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false)
                                    .Cast<KeyAttribute>().FirstOrDefault();
                                break;
                            }
                        }
                    }
                }

                if (columnName == null)
                {
                    continue;
                }

                string parameterName = '@' + propertyName;

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

            return @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] SET " +
                   string.Join(", ", parameterList) + " WHERE " + string.Join(" AND ", conditionList);
        }

        private string CreateUpdateStatement(object parameters, object conditions,
            out IList<SqlParameter> sqlParameterList)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
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

            string query = @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableName<T>() + "] SET " +
                           string.Join(", ", parameterList);

            if (conditionList.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditionList);
            }

            return query;
        }

        private string CreateUpdateStatement(IDictionary<string, object> parameters,
            IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList)
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object value = parameter.Value;
                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                string propertyName = parameter.Key.TrimStart('@');
                string columnName = null;
                
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
                parameterList.Add("[" + columnName + "] = " + parameterName);
            }

            foreach (KeyValuePair<string, object> condition in conditions)
            {
                object value = condition.Value;
                if (value == null && IgnoreNulls)
                {
                    continue;
                }

                string propertyName = condition.Key.TrimStart('@');
                string columnName = null;

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
                conditionList.Add("[" + columnName + "] = " + parameterName);
            }
            return @"UPDATE " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableName<T>() + "] SET " +
                   string.Join(", ", parameterList) + " WHERE " + string.Join(" AND ", conditionList);
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

        private SqlCommand CreateCommandSelect(IEnumerable<string> columns, string orderBy = null, int? page = null,
            int? pageSize = null)
        {
            string query = CreateSelectStatement(columns, out IList<SqlParameter> sqlParameterList, orderBy, page,
                pageSize);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandSelect(IEnumerable<string> columns, object conditions, string orderBy = null,
            int? page = null, int? pageSize = null)
        {
            string query = CreateSelectStatement(columns, conditions, out IList<SqlParameter> sqlParameterList, orderBy,
                page, pageSize);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandSelect(IEnumerable<string> columns, IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            string query = CreateSelectStatement(columns, conditions, out IList<SqlParameter> sqlParameterList, orderBy,
                page, pageSize);
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
            string query = CreateInsertStatement(parameters, out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandInsert(IDictionary<string, object> parameters)
        {
            string query = CreateInsertStatement(parameters, out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);

            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());

            return command;
        }

        private SqlCommand CreateCommandUpdate(object parameters)
        {
            string query = CreateUpdateStatement(parameters, out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(IDictionary<string, object> parameters)
        {
            string query = CreateUpdateStatement(parameters, out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(object parameters, object conditions)
        {
            string query = CreateUpdateStatement(parameters, conditions, out IList<SqlParameter> sqlParameterList);
            SqlCommand command = Transaction != null
                ? new SqlCommand(query, Connection, Transaction)
                : new SqlCommand(query, Connection);
            if (sqlParameterList.Count > 0)
                command.Parameters.AddRange(sqlParameterList.ToArray());
            return command;
        }

        private SqlCommand CreateCommandUpdate(IDictionary<string, object> parameters, IDictionary<string, object> conditions)
        {
            string query = CreateUpdateStatement(parameters, conditions, out IList<SqlParameter> sqlParameterList);
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
    }
}