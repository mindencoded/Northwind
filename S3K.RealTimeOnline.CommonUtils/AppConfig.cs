using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace S3K.RealTimeOnline.CommonUtils
{
    public class AppConfig
    {
        public static string DbDateFormat => ConfigurationManager.AppSettings["DbDateFormat"];

        public static string SecurityDbConnectionName => ConfigurationManager.AppSettings["SecurityDbConnectionName"];

        public static string BusinessDbConnectionName => ConfigurationManager.AppSettings["BusinessDbConnectionName"];

        public static bool EnableSecurity => Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSecurity"]);

        public static SslFlag[] SslFlags
        {
            get
            {
                IList<SslFlag> resultList = new List<SslFlag>();
                string[] sslFlags = ConfigurationManager.AppSettings["SslFlags"].Split(',');
                foreach (SslFlag sslFlag in Enum.GetValues(typeof(SslFlag)))
                {
                    if (sslFlags.Contains(sslFlag.ToString()))
                    {
                        resultList.Add(sslFlag);
                    }
                }
                if (!sslFlags.Any())
                {
                    resultList.Add(SslFlag.None);
                }
                return resultList.ToArray();
            }
        }
    }
}