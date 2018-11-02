using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Northwind.WebRole.Domain;

namespace Northwind.WebRole.Utils
{
    public class StatementBuilder
    {
        private readonly bool _ignoreNulls;

        public StatementBuilder()
        {
            _ignoreNulls = true;
        }

        public StatementBuilder(bool ignoreNulls)
        {
            _ignoreNulls = ignoreNulls;
        }

        public string CreateSelectStatement<T>() where T : class
        {
            return @"SELECT " + EntityUtils.JoinColumns<T>() + " FROM " + EntityUtils.GetSchema<T>() + ".[" +
                   EntityUtils.GetTableOrViewName<T>() + "]";
        }

        public string CreateSelectStatement<T>(
            IList<string> columns,
            out IList<SqlParameter> sqlParameterList,
            string orderBy = null,
            int? page = null, int?
                pageSize = null) where T : class
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
                    columnQuery = EntityUtils.JoinColumns<T>(false, true);
                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>();
                }
            }

            sqlParameterList = new List<SqlParameter>();


            string query = @"SELECT " + columnQuery + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableOrViewName<T>() + "]";


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

        public string CreateSelectStatement<T>(
            IList<string> columns,
            IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList,
            string orderBy = null,
            int? page = null, int?
                pageSize = null) where T : class
        {
            string columnQuery;

            if (columns != null)
            {
                if (page != null && pageSize != null)
                {
                    columnQuery = EntityUtils.JoinColumns<T>(columns, true, true);
                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>(columns, true);
                }
            }
            else
            {
                if (page != null && pageSize != null)
                {
                    columnQuery = EntityUtils.JoinColumns<T>(false, true);
                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>();
                }
            }

            string query = @"SELECT " + columnQuery + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableOrViewName<T>() + "]";

            sqlParameterList = new List<SqlParameter>();

            if (conditions != null && conditions.Any())
            {
                string where = CreateWhereStatement<T>(conditions, out sqlParameterList);
                if (where != null)
                {
                    query += where;
                }
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

                columnQuery = columns != null
                    ? EntityUtils.SimpleJoinColumns<T>(columns, true)
                    : EntityUtils.SimpleJoinColumns<T>();

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

        public string CreateSelectStatement<T>(
            IList<string> columns,
            IList<ParameterBuilder> conditions,
            out IList<SqlParameter> sqlParameterList,
            string orderBy = null,
            int? page = null, int?
                pageSize = null) where T : class
        {
            string columnQuery;



            if (columns != null)
            {
                if (page != null && pageSize != null)
                {
                    columnQuery = EntityUtils.JoinColumns<T>(columns, true, true);
                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>(columns, true);
                }
            }
            else
            {
                if (page != null && pageSize != null)
                {
                    columnQuery = EntityUtils.JoinColumns<T>(true, true);
                }
                else
                {
                    columnQuery = EntityUtils.JoinColumns<T>(true);
                }
            }

            string query = @"SELECT " + columnQuery + " FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableOrViewName<T>() + "]";

            sqlParameterList = new List<SqlParameter>();

            if (conditions != null && conditions.Any())
            {
                string where = CreateWhereStatement<T>(conditions, out sqlParameterList);
                if (where != null)
                {
                    query += where;
                }
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

                columnQuery = columns != null
                    ? EntityUtils.SimpleJoinProperties<T>(columns)
                    : EntityUtils.SimpleJoinProperties<T>();

                query = @"WITH PageNumbers AS (" + query + ") SELECT " + columnQuery +
                        " FROM  PageNumbers WITH (NOLOCK) WHERE RowNumber BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)";
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query += " ORDER BY " + orderBy;
            }

            return query;
        }


        public string CreateWhereStatement<T>(
            IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            int count = 0;
            foreach (KeyValuePair<string, object> condition in conditions)
            {
                object value = condition.Value;
                if (value == null && _ignoreNulls)
                {
                    continue;
                }

                string columnName = null;
                string propertyName = condition.Key.TrimStart('@'); //entry.Key.Replace("@", "").Trim();
                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo propertyInfo = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        propertyInfo.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                        .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                    if (columnAttributes.Select(x => x.Name).Contains(propertyName))
                    {
                        columnName = propertyName;
                    }
                }

                if (columnName == null)
                {
                    continue;
                }

                string parameterName = "@p" + count;
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                if (value != null && value.GetType().IsGenericType &&
                    value.GetType().GetGenericTypeDefinition() == typeof(Tuple<,>) && value is DateTime)
                {
                    conditionList.Add("(" + EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableOrViewName<T>() +
                                      "].[" + columnName + "] BETWEEN " + parameterName + "_1 AND " + parameterName +
                                      "_2)");
                    FieldInfo[] fields = value.GetType().GetFields();
                    for (int i = 1; i <= fields.Length; i++)
                    {
                        parameters.Add(parameterName + "_" + i, fields[i].GetValue(value));
                    }
                }
                else if (value != null && value.GetType().IsArray)
                {
                    object[] items = (object[])value;
                    IList<string> subConditionList = new List<string>();
                    if (typeof(string).IsAssignableFrom(value.GetType().GetElementType()))
                    {
                        for (int i = 0; i < items.Length; i++)
                        {
                            subConditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableOrViewName<T>() +
                                                 "].[" + columnName + "] = " + parameterName + "_" + i);
                            parameters.Add(parameterName + "_" + i, items[i]);
                        }

                        conditionList.Add("(" + string.Join(" OR ", subConditionList) + ")");
                    }
                    else
                    {
                        for (int i = 0; i < items.Length; i++)
                        {
                            subConditionList.Add(parameterName + "_" + i);
                            parameters.Add(parameterName + "_" + i, items[i]);
                        }

                        conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableOrViewName<T>() +
                                          "].[" + columnName + "] IN (" + string.Join(", ", subConditionList) +
                                          ")");
                    }
                }
                else if (value != null && value.ToString().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableOrViewName<T>() + "].[" +
                                      columnName + "] IS NULL");
                }
                else
                {
                    conditionList.Add(EntityUtils.GetSchema<T>() + ".[" + EntityUtils.GetTableOrViewName<T>() + "].[" +
                                      columnName + "] = " + parameterName);
                    parameters.Add(parameterName, value);
                }

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameter.Key,
                        SourceColumn = columnName,
                        Value = parameter.Value ?? DBNull.Value
                    };

                    if (value != null)
                    {
                        sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType().IsArray
                            ? value.GetType().GetElementType()
                            : value.GetType());
                    }

                    sqlParameterList.Add(sqlParameter);
                }

                count++;
            }

            if (conditionList.Count > 0)
            {
                return " WHERE " + string.Join(" AND ", conditionList);
            }

            return null;
        }

        public string CreateWhereStatement<T>(IList<ParameterBuilder> conditions, out IList<SqlParameter> sqlParameterList) where T : class
        {
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            int count = 0;
            foreach (ParameterBuilder condition in conditions)
            {
                if (string.IsNullOrWhiteSpace(condition.PropertyName)) continue;
                object value = condition.Value;
                if (value == null && _ignoreNulls) continue;
                string columnName = null;
                string propertyName = condition.PropertyName.TrimStart('@'); //entry.Key.Replace("@", "").Trim();
                string parameterName = "@p" + count;
                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                        .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                    if (columnAttributes.Select(x => x.Name).Contains(propertyName))
                    {
                        columnName = propertyName;
                    }
                }

                if (columnName == null)
                {
                    continue;
                }

                IDictionary<string, object> parameters = new Dictionary<string, object>();

                if (condition.Comparison == Comparison.Between)
                {
                    bool typeValid = false;
                    if (value != null)
                    {
                        if (value.GetType().IsGenericType &&
                            value.GetType().GetGenericTypeDefinition() == typeof(Tuple<,>))
                        {
                            FieldInfo[] fields = value.GetType().GetFields();
                            for (int i = 1; i <= fields.Length; i++)
                            {
                                parameters.Add(parameterName + "_" + i, fields[i].GetValue(value));
                            }

                            typeValid = true;
                        }
                        else if (value.GetType().IsArray)
                        {
                            object[] items = (object[])value;
                            if (items.Length >= 2)
                            {
                                parameters.Add(parameterName + "_1", items[0]);
                                parameters.Add(parameterName + "_2", items[1]);
                            }

                            typeValid = true;
                        }

                        if (typeValid)
                        {
                            conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                              EntityUtils.GetSchema<T>() + ".[" +
                                              EntityUtils.GetTableOrViewName<T>() +
                                              "].[" + columnName + "] BETWEEN  " + parameterName + "_1" +
                                              " AND " +
                                              parameterName + "_2" + condition.EndGroup());
                        }
                    }
                }
                else if (condition.Comparison == Comparison.Contains)
                {
                    if (value != null)
                    {
                        if (value.GetType().IsArray)
                        {
                            object[] items = (object[])value;

                            for (int i = 0; i < items.Length; i++)
                            {
                                parameters.Add(parameterName + "_" + i, items[i]);
                            }

                            conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                              EntityUtils.GetSchema<T>() + ".[" +
                                              EntityUtils.GetTableOrViewName<T>() +
                                              "].[" + columnName + "] IN (" +
                                              string.Join(", ", parameters.Keys) + ")" + condition.EndGroup());
                        }
                        else
                        {
                            parameters.Add(parameterName, value);
                            conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                              EntityUtils.GetSchema<T>() + ".[" +
                                              EntityUtils.GetTableOrViewName<T>() +
                                              "].[" + columnName + "] IN (" +
                                              string.Join(", ", parameterName) + ")" + condition.EndGroup());
                        }
                    }
                }
                else if (condition.Comparison == Comparison.Like || condition.Comparison == Comparison.StartsWith ||
                         condition.Comparison == Comparison.EndsWith)
                {
                    if (value != null)
                    {
                        string comparison;
                        if (value.GetType().IsArray &&
                            typeof(string).IsAssignableFrom(value.GetType().GetElementType()))
                        {
                            object[] items = (object[])value;
                            IList<string> subConditionList = new List<string>();
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (condition.Comparison == Comparison.StartsWith)
                                {
                                    comparison = parameterName + "_" + i +
                                                 " + '%'";
                                }
                                else if (condition.Comparison == Comparison.EndsWith)
                                {
                                    comparison = "'%' + " +
                                                 parameterName + "_" + i;
                                }
                                else
                                {
                                    comparison = "'%' + " +
                                                 parameterName + "_" + i +
                                                 " + '%'";
                                }

                                subConditionList.Add(EntityUtils.GetSchema<T>() + ".[" +
                                                     EntityUtils.GetTableOrViewName<T>() +
                                                     "].[" + columnName + "] LIKE " + comparison);
                                parameters.Add(parameterName + "_" + i, items[i]);
                            }

                            conditionList.Add(condition.Condition + " " + condition.StartGroup() + "(" +
                                              string.Join(" OR ", subConditionList) +
                                              ")" + condition.EndGroup());
                        }
                        else
                        {
                            if (condition.Comparison == Comparison.StartsWith)
                            {
                                comparison = parameterName +
                                             " + '%'";
                            }
                            else if (condition.Comparison == Comparison.EndsWith)
                            {
                                comparison = "'%' + " +
                                             parameterName;
                            }
                            else
                            {
                                comparison = "'%' + " +
                                             parameterName +
                                             " + '%'";
                            }

                            conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                              EntityUtils.GetSchema<T>() + ".[" +
                                              EntityUtils.GetTableOrViewName<T>() +
                                              "].[" + columnName + "] LIKE " + comparison + condition.EndGroup());
                            parameters.Add(parameterName, value);
                        }
                    }
                }
                else if (value != null && value.ToString().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    if (condition.Comparison == Comparison.NotEqualTo)
                    {
                        conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                          EntityUtils.GetSchema<T>() + ".[" +
                                          EntityUtils.GetTableOrViewName<T>() +
                                          "].[" + columnName + "] IS NOT NULL");
                    }
                    else
                    {
                        conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                          EntityUtils.GetSchema<T>() + ".[" +
                                          EntityUtils.GetTableOrViewName<T>() +
                                          "].[" + columnName + "] IS NULL");
                    }
                }
                else
                {
                    conditionList.Add(condition.Condition + " " + condition.StartGroup() +
                                      EntityUtils.GetSchema<T>() + ".[" +
                                      EntityUtils.GetTableOrViewName<T>() +
                                      "].[" + columnName + "] " + condition.Comparison.ToSqlComparison() + " " +
                                      parameterName + condition.EndGroup());


                    parameters.Add(parameterName, value);
                }

                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    SqlParameter sqlParameter = new SqlParameter
                    {
                        ParameterName = parameter.Key,
                        SourceColumn = columnName,
                        Value = parameter.Value ?? DBNull.Value
                    };

                    if (value != null)
                    {
                        if (value.GetType().IsArray && ((object[])value).Length > 0)
                        {
                            sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(((object[])value)[0].GetType());
                        }
                        else
                        {
                            sqlParameter.SqlDbType = TypeConvertor.ToSqlDbType(value.GetType());
                        }
                    }

                    sqlParameterList.Add(sqlParameter);
                }

                count++;
            }

            if (conditionList.Count > 0)
            {
                return " WHERE" + string.Join(" ", conditionList).TrimStart("Or").TrimStart("And");
            }

            return null;
        }

        public string CreateCountStatement<T>(
            IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            string query = @"SELECT COUNT(*) FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableOrViewName<T>() + "]";
            sqlParameterList = new List<SqlParameter>();
            if (conditions != null && conditions.Any())
            {
                string where = CreateWhereStatement<T>(conditions, out sqlParameterList);
                if (where != null)
                {
                    query += where;
                }
            }

            return query;
        }

        public string CreateCountStatement<T>(
            IList<ParameterBuilder> conditions,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            sqlParameterList = new List<SqlParameter>();

            string query = @"SELECT COUNT(*) FROM " +
                           EntityUtils.GetSchema<T>() + ".[" +
                           EntityUtils.GetTableOrViewName<T>() + "]";

            sqlParameterList = new List<SqlParameter>();

            if (conditions != null && conditions.Any())
            {
                string where = CreateWhereStatement<T>(conditions, out sqlParameterList);
                if (where != null)
                {
                    query += where;
                }
            }

            return query;
        }

        public string CreateSelectByIdStatement<T>(object id, out IList<SqlParameter> sqlParameterList) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            string parameterName = null;
            Attribute keyAttribute = null;
            string columnName = null;
            sqlParameterList = new List<SqlParameter>();
            foreach (PropertyInfo property in properties)
            {
                parameterName = '@' + property.Name;
                keyAttribute = property.GetCustomAttribute(typeof(KeyAttribute));
                if (keyAttribute != null)
                {
                    ColumnAttribute columnAttribute =
                        property.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
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
            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = parameterName,
                SourceColumn = columnName,
                SqlDbType = TypeConvertor.ToSqlDbType(id.GetType()),
                Value = id
            };
            sqlParameterList.Add(sqlParameter);
            return query;
        }

        public string CreateInsertStatement<T>(object parameters, bool isIdentityInsert, out IList<SqlParameter> sqlParameterList) where T : class
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
                if (keyAttribute != null && isIdentityInsert)
                {
                    continue;
                }

                object value = property.GetValue(parameters, null);
                if (value == null && _ignoreNulls)
                {
                    continue;
                }

                string parameterName = '@' + typeProperty.Name;
                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
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

        public string CreateInsertStatement<T>(IDictionary<string, object> parameters,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            IList<string> columnNameList = new List<string>();
            sqlParameterList = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object value = parameter.Value;
                if (value == null && _ignoreNulls)
                {
                    continue;
                }

                string propertyName = parameter.Key.TrimStart('@');
                string columnName = null;
                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                        .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                    if (columnAttributes.Select(x => x.Name).Contains(propertyName))
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

        public string CreateUpdateStatement<T>(object parameters, out IList<SqlParameter> sqlParameterList) where T : class
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

                if (value == null && _ignoreNulls)
                {
                    continue;
                }

                PropertyInfo typeProperty = typeProperties.First(x => x.Name == property.Name);
                string parameterName = '@' + typeProperty.Name;
                ColumnAttribute columnAttribute =
                    typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();

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

        public string CreateUpdateStatement<T>(IDictionary<string, object> parameters,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object value = parameter.Value;
                if (value == null && _ignoreNulls)
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
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                    keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                }
                else
                {
                    IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                        .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                    if (columnAttributes.Select(x => x.Name).Contains(propertyName))
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

                if (columnName.ToUpper().Equals("CREATED"))
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

        public string CreateUpdateStatement<T>(object parameters, object conditions,
            out IList<SqlParameter> sqlParameterList) where T : class
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
                    typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();

                string columnName = columnAttribute != null ? columnAttribute.Name : typeProperty.Name;

                if (columnName.ToUpper().Equals("CREATED"))
                {
                    continue;
                }

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

                    if (value == null && _ignoreNulls)
                    {
                        continue;
                    }

                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();

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

        public string CreateUpdateStatement<T>(IDictionary<string, object> parameters,
            IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> parameterList = new List<string>();
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                object value = parameter.Value;
                if (value == null && _ignoreNulls)
                {
                    continue;
                }

                string propertyName = parameter.Key.TrimStart('@');
                string columnName = null;

                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                        .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                    if (columnAttributes.Select(x => x.Name).Contains(propertyName))
                    {
                        columnName = propertyName;
                    }
                }

                if (columnName == null)
                {
                    continue;
                }

                if (columnName.ToUpper().Equals("CREATED"))
                {
                    continue;
                }

                string parameterName = '@' + propertyName;
                parameterList.Add("[" + columnName + "] = " + parameterName);
            }

            foreach (KeyValuePair<string, object> condition in conditions)
            {
                object value = condition.Value;
                if (value == null && _ignoreNulls)
                {
                    continue;
                }

                string propertyName = condition.Key.TrimStart('@');
                string columnName = null;

                if (typeProperties.Select(x => x.Name).Contains(propertyName))
                {
                    PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                        .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                    if (columnAttributes.Select(x => x.Name).Contains(propertyName))
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

        public string CreateDeleteStatement<T>(object conditions, out IList<SqlParameter> sqlParameterList) where T : class
        {
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
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

                    if (value == null && _ignoreNulls)
                    {
                        continue;
                    }

                    string parameterName = '@' + typeProperty.Name;

                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();

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

            return query;
        }

        public string CreateDeleteStatement<T>(IDictionary<string, object> conditions,
            out IList<SqlParameter> sqlParameterList) where T : class
        {
            IList<string> conditionList = new List<string>();
            sqlParameterList = new List<SqlParameter>();
            if (conditions != null)
            {
                Type type = typeof(T);
                PropertyInfo[] typeProperties = type.GetProperties();
                foreach (KeyValuePair<string, object> condition in conditions)
                {
                    object value = condition.Value;
                    if (value == null && _ignoreNulls)
                    {
                        continue;
                    }

                    string propertyName = condition.Key.TrimStart('@');
                    string columnName = null;

                    if (typeProperties.Select(x => x.Name).Contains(propertyName))
                    {
                        PropertyInfo typeProperty = typeProperties.First(x => x.Name == propertyName);
                        ColumnAttribute columnAttribute =
                            typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                        columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                    }
                    else
                    {
                        IList<ColumnAttribute> columnAttributes = typeof(T).GetProperties()
                            .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()).ToList();
                        if (columnAttributes.Select(x => x.Name).Contains(propertyName))
                        {
                            columnName = propertyName;
                        }
                    }

                    if (columnName == null)
                    {
                        continue;
                    }

                    string parameterName = '@' + propertyName;

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

            return query;
        }

        public string CreateDeleteStatementById<T>(object id, out string propertyName) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            string condition = null;
            propertyName = null;
            if (id != null)
            {
                foreach (PropertyInfo typeProperty in typeProperties)
                {
                    Attribute keyAttribute = typeProperty.GetCustomAttribute(typeof(KeyAttribute));
                    if (keyAttribute == null) continue;
                    propertyName = '@' + typeProperty.Name;
                    ColumnAttribute columnAttribute =
                        typeProperty.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
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

            return query;
        }
    }
}