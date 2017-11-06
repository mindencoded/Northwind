using System.Configuration;
using System.Data.SqlClient;

namespace S3K.RealTimeOnline.DataAccess.Tools
{
    public class DbManager
    {
        public static readonly string DateFormat = ConfigurationManager.AppSettings["DbDateFormat"];
        
        public static int ConnectionTimeOut { get; set; }

        public static string ApplicationName { get; set; }

        public static string Server { get; private set; }

        public static string DatabaseName { get; private set; }

        public static string Name { get; private set; }

        public static string ProviderName { get; private set; }

        public static string ConnectionString { get; private set; }

        public static string GetConnectionString(string connectionName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionName];
            ProviderName = connectionStringSettings.ProviderName;
            Name = connectionStringSettings.Name;
            ConnectionString = connectionStringSettings.ConnectionString;    
            var builder = CreateSqlConnectionStringBuilder(ConnectionString);
            return builder.ToString();
        }

        public static SqlConnectionStringBuilder CreateSqlConnectionStringBuilder(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.ApplicationName = ApplicationName ?? builder.ApplicationName;
            Server = builder.DataSource;
            DatabaseName = builder.InitialCatalog;
            builder.ConnectTimeout = ConnectionTimeOut > 0 ? ConnectionTimeOut : builder.ConnectTimeout;
            return builder;
        }

        public static SqlConnection GetSqlConnection(string connectionName)
        {
            var connectionString = GetConnectionString(connectionName);
            return new SqlConnection(connectionString);
        }
    }
}