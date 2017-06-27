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

        public static string GetConnectionString(string connectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ToString();
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.ApplicationName = ApplicationName ?? builder.ApplicationName;
            Server = builder.DataSource;
            DatabaseName = builder.InitialCatalog;
            builder.ConnectTimeout = ConnectionTimeOut > 0 ? ConnectionTimeOut : builder.ConnectTimeout;
            return builder.ToString();
        }

        public static SqlConnection GetSqlConnection(string connectionName)
        {
            var connectionString = GetConnectionString(connectionName);
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}