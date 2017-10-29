using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace S3K.RealTimeOnline.DataAccess.Tools
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
                var name = reader.GetName(i);
                var value = reader[i];

                /*
                var dotNetType = reader.GetFieldType(i);
                var sqlType = reader.GetDataTypeName(i);
                */

                if (value is DBNull) continue;

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        ColumnAttribute attribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        if (attribute != null && (name == attribute.Name || name == property.Name))
                        {
                            property.SetValue(instance, value);
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            return instance;
        }

        public static IEnumerable<T> ConvertToEnumerable<T>(this SqlDataReader reader)
        {
            Type type = typeof(T);
            T instance = (T)Activator.CreateInstance(type);
            PropertyInfo[] properties = type.GetProperties();
            IList<T> result = new List<T>();
            while(reader.Read())
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

        public static dynamic ConvertToDynamic(this SqlDataReader reader)
        {
            IList<string> names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (IDataRecord record in reader as IEnumerable)
            {
                foreach (var name in names)
                {
                    expando[name] = record[name];
                }

            }
            return expando;
        }

        public static IEnumerable<dynamic> ConvertToDynamicEnumerable(this SqlDataReader reader)
        {
            IList<string> names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            IList<dynamic> result = new List<dynamic>();
            foreach (IDataRecord record in reader as IEnumerable)
            {
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (var name in names)
                {
                    expando[name] = record[name];
                }
                result.Add(expando);
            }
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> ConvertToDictionaryEnumerable(this SqlDataReader reader)
        {
            var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            foreach (IDataRecord record in reader as IEnumerable)
                yield return names.ToDictionary(name => name, name => record[name]);
        }

        public static IEnumerable<ExpandoObject> ConvertToExpandoEnumerable(this SqlDataReader reader)
        {
            while (reader.Read())
            {
                yield return ConvertToExpando(reader);
            }
        }

        public static ExpandoObject ConvertToExpando(IDataRecord record)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < record.FieldCount; i++)
                expandoObject.Add(record.GetName(i), record[i]);

            return (ExpandoObject) expandoObject;
        }

        public static IEnumerable<T> GetSqlData<T>(this SqlDataReader reader)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            IList<T> result = new List<T>();
            while (reader.Read())
            {
                T instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    ColumnAttribute attribute =
                        propertyInfo.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    object o = null;
                    if (attribute != null)
                    {
                        try
                        {
                            o = reader[attribute.Name];
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (o == null)
                    {
                        try
                        {
                            o = reader[propertyInfo.Name];
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (o != null && o.GetType() != typeof(DBNull))
                    {
                        propertyInfo.SetValue(instance, o, null);
                    }
                }
                result.Add(instance);
            }
            return result;
        }
    }
}