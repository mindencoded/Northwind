using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using S3K.RealTimeOnline.Commons;
using S3K.RealTimeOnline.Domain;

namespace S3K.RealTimeOnline.DataAccess.Tools
{
    public class InformationSchema
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        public InformationSchema(SqlConnection connection)
        {
            _connection = connection;
        }

        public InformationSchema(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IList<ColumnInfo> Columns<T>() where T :class
        {
            string query = @"
                SELECT 
                    COLUMN_NAME,
                    ORDINAL_POSITION, 
                    IS_NULLABLE,
                    DATA_TYPE, 
                    CHARACTER_MAXIMUM_LENGTH, 
                    NUMERIC_PRECISION, 
                    NUMERIC_PRECISION_RADIX, 
                    NUMERIC_SCALE, 
                    DATETIME_PRECISION
                FROM 
                    information_schema.columns
                WHERE 
                    Table_Name = '" + EntityUtils.GetTableName<T>() + "'";
            IList<ColumnInfo> results;
            SqlCommand command = _transaction != null
                ? new SqlCommand(query, _connection, _transaction)
                : new SqlCommand(query, _connection);
            using (command)
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    results = new List<ColumnInfo>();
                    while (reader.Read())
                        results.Add(reader.ConvertTo<ColumnInfo>());
                }
            }
            return results;
        }

        public ColumnInfo Column<T>(string columnName) where T : class
        {
            string query = @"
                SELECT 
                    COLUMN_NAME,
                    ORDINAL_POSITION, 
                    IS_NULLABLE,
                    DATA_TYPE, 
                    CHARACTER_MAXIMUM_LENGTH, 
                    NUMERIC_PRECISION, 
                    NUMERIC_PRECISION_RADIX, 
                    NUMERIC_SCALE, 
                    DATETIME_PRECISION
                FROM 
                    information_schema.columns
                WHERE 
                    Table_Name = '" + EntityUtils.GetTableName<T>() + "' AND Column_Name = '" + columnName + "'";
            ColumnInfo columnInfo = null;
            SqlCommand command = _transaction != null
                ? new SqlCommand(query, _connection, _transaction)
                : new SqlCommand(query, _connection);
            using (command)
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columnInfo = reader.ConvertTo<ColumnInfo>();
                    }
                }
            }
            return columnInfo;
        }
    }
}
