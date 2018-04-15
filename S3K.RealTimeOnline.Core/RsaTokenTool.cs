using System.IO;
using System.Security.Cryptography;

namespace S3K.RealTimeOnline.Core
{
    public class RsaTokenTool
    {
        public static void CreateKeys(int dwKeySize, string keyContainerName, string privateAndPublicKeyPath, string publicKeyPath)
        {
            //stream to save the keys
            FileStream fs = null;
            StreamWriter sw = null;
            string xmlString;
            //create RSA provider
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwKeySize,
                new CspParameters
                {
                    KeyContainerName = keyContainerName
                });
            try
            {
                fs = new FileStream(privateAndPublicKeyPath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                xmlString = rsa.ToXmlString(true);
                sw.Write(xmlString);
                sw.Flush();

                fs = new FileStream(publicKeyPath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                xmlString = rsa.ToXmlString(true);
                sw.Write(xmlString);
                sw.Flush();
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
            rsa.Clear();
        }

        public static void CreatePrivateAndPublicKeys(int dwKeySize, string keyContainerName, string privateAndPublicKeyPath, string publicKeyPath)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwKeySize,
                new CspParameters
                {
                    KeyContainerName = keyContainerName
                });
            string xmlPrivateKey = rsa.ToXmlString(true);
            string xmlPublicKey = rsa.ToXmlString(false);
            File.WriteAllText(privateAndPublicKeyPath, xmlPrivateKey);
            File.WriteAllText(publicKeyPath, xmlPublicKey);
        }

        public static void CreatePrivateAndPublicKeys(int dwKeySize, string keyContainerName, string privateAndPublicKeyPath)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwKeySize,
                new CspParameters
                {
                    KeyContainerName = keyContainerName
                });
            string xmlPrivateKey = rsa.ToXmlString(true);
            File.WriteAllText(privateAndPublicKeyPath, xmlPrivateKey);
        }

        public static RSACryptoServiceProvider LoadPrivateAndPublicKeys(int dwKeySize, string keyContainerName, string privateAndPublicKeyPath)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwKeySize,
                new CspParameters
                {
                    KeyContainerName = keyContainerName
                });
            string xmlPrivateKey = File.ReadAllText(privateAndPublicKeyPath);
            rsa.FromXmlString(xmlPrivateKey);
            return rsa;
        }
        
    }
}
