using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Northwind.WebRole.Tools
{
    public static class SqlDataReaderExtensions
    {
        public static T ConvertTo<T>(this SqlDataReader reader)
        {
            Type type = typeof(T);
            T instance = (T) Activator.CreateInstance(type);
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                object value = reader[i];

                /*
                Type dotNetType = reader.GetFieldType(i);
                string sqlType = reader.GetDataTypeName(i);
                */

                if (value is DBNull) continue;

                foreach (PropertyInfo property in properties)
                {
                    ColumnAttribute attribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    if (attribute != null && name == attribute.Name || name == property.Name)
                    {
                        property.SetValue(instance, value);
                        break;
                    }
                }
            }

            return instance;
        }

        public static async Task<T> ConvertToAsync<T>(this SqlDataReader reader)
        {
            Type type = typeof(T);
            T instance = (T) Activator.CreateInstance(type);
            PropertyInfo[] properties = type.GetProperties();
            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    object value = await reader.GetFieldValueAsync<object>(i);
                    if (value is DBNull) continue;
                    foreach (PropertyInfo property in properties)
                    {
                        ColumnAttribute attribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        if (attribute != null && name == attribute.Name || name == property.Name)
                        {
                            property.SetValue(instance, value);
                            break;
                        }
                    }
                }
            }

            return instance;
        }

        public static IList<T> ConvertToList<T>(this SqlDataReader reader)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<T> result = new List<T>();
            while (reader.Read())
            {
                T instance = (T) Activator.CreateInstance(type);
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        ColumnAttribute attribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        string name = attribute != null ? attribute.Name : property.Name;
                        object value = reader[name];
                        if (value is DBNull) continue;
                        property.SetValue(instance, value);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                result.Add(instance);
            }

            return result;
        }

        public static async Task<IList<T>> ConvertToListAsync<T>(this SqlDataReader reader)
        {
            Type type = typeof(T);
            T instance = (T) Activator.CreateInstance(type);
            PropertyInfo[] properties = type.GetProperties();
            IList<T> result = new List<T>();
            while (await reader.ReadAsync())
            {
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        ColumnAttribute attribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        string name = attribute != null ? attribute.Name : property.Name;
                        object value = reader[name];
                        if (value is DBNull) continue;
                        property.SetValue(instance, value);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                result.Add(instance);
            }

            return result;
        }

        public static IList<ExpandoObject> ConvertToDynamicList(this SqlDataReader reader)
        {
            IList<ExpandoObject> expandoList = new List<ExpandoObject>();
            while (reader.Read())
            {
                expandoList.Add(ConvertToExpando(reader));
            }

            return expandoList;
        }

        public static async Task<IList<ExpandoObject>> ConvertToDynamicListAsync(this SqlDataReader reader)
        {
            IList<ExpandoObject> expandoList = new List<ExpandoObject>();
            while (await reader.ReadAsync())
            {
                expandoList.Add(ConvertToExpando(reader));
            }

            return expandoList;
        }

        public static ExpandoObject ConvertToExpando(IDataRecord record)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject();

            for (int i = 0; i < record.FieldCount; i++)
                expandoObject.Add(record.GetName(i), record[i]);

            return (ExpandoObject) expandoObject;
        }
    }
}