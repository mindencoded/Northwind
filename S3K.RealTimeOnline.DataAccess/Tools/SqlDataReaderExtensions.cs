﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

        public static IList<T> ConvertToList<T>(this SqlDataReader reader)
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

        public static async Task<IList<T>> ConvertToListAsync<T>(this SqlDataReader reader)
        {
            Type type = typeof(T);
            T instance = (T)Activator.CreateInstance(type);
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

        public static IList<dynamic> ConvertToDynamicList(this SqlDataReader reader)
        {
           
            IList<dynamic> result = new List<dynamic>();

            IList<string> names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
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
    }
}