using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace S3K.RealTimeOnline.CommonUtils
{
    public class AppConfig
    {
        public static string SecurityDbConnectionName => ConfigurationManager.AppSettings["SecurityDbConnectionName"];

        public static string BusinessDbConnectionName => ConfigurationManager.AppSettings["BusinessDbConnectionName"];

        public static string BaseUri => ConfigurationManager.AppSettings["BaseUri"];

        public static string DbDateFormat => ConfigurationManager.AppSettings["DbDateFormat"];

        public static bool EnableSecurity => Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSecurity"]);

        public static SslFlag[] SslFlags
        {
            get
            {
                IList<SslFlag> resultList = new List<SslFlag>();
                string[] sslFlags = ConfigurationManager.AppSettings["SslFlags"].Split(',').Select(p => p.Trim())
                    .ToArray();
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

        public static StoreLocation StoreLocation
        {
            get
            {
                string value = ConfigurationManager.AppSettings["StoreLocation"];
                foreach (StoreLocation storeLocation in Enum.GetValues(typeof(StoreLocation)))
                {
                    if (storeLocation.ToString() == value)
                    {
                        return storeLocation;
                    }
                }
                return StoreLocation.LocalMachine;
            }
        }

        public static StoreName StoreName
        {
            get
            {
                string value = ConfigurationManager.AppSettings["StoreName"];
                foreach (StoreName storeName in Enum.GetValues(typeof(StoreName)))
                {
                    if (storeName.ToString() == value)
                    {
                        return storeName;
                    }
                }
                return StoreName.My;
            }
        }

        public static X509FindType X509FindType
        {
            get
            {
                string value = ConfigurationManager.AppSettings["X509FindType"];
                foreach (X509FindType x509FindType in Enum.GetValues(typeof(X509FindType)))
                {
                    if (x509FindType.ToString() == value)
                    {
                        return x509FindType;
                    }
                }
                return X509FindType.FindBySubjectName;
            }
        }

        public static string FindValue => ConfigurationManager.AppSettings["FindValue"];

        public static double TokenExpirationMinutes =>
            double.Parse(ConfigurationManager.AppSettings["TokenExpirationMinutes"]);

        public static string RsaPrivateKeyXml => ConfigurationManager.AppSettings["RsaPrivateKeyXml"];

        public static string RsaPublicKeyXml => ConfigurationManager.AppSettings["RsaPublicKeyXml"];

        //public static string BinDirectory => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        public static bool UseRsa => Convert.ToBoolean(ConfigurationManager.AppSettings["UseRsa"]);

        public static string HmacSecretKey => ConfigurationManager.AppSettings["HmacSecretKey"];
    }
}