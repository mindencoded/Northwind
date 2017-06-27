using System.Configuration;

namespace S3K.RealTimeOnline.Commons
{
    public class AppConfig
    {
        public static string DbDateFormat
        {
            get { return ConfigurationManager.AppSettings["DbDateFormat"]; }
        }

        public static string ConnectionName
        {
            get { return ConfigurationManager.AppSettings["ConnectionName"]; }
        }
    }
}