using System.Configuration;

namespace S3K.RealTimeOnline.Commons
{
    public class AppConfig
    {
        public static string DbDateFormat => ConfigurationManager.AppSettings["DbDateFormat"];

        public static string ConnectionName => ConfigurationManager.AppSettings["ConnectionName"];
    }
}