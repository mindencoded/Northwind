using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace S3K.RealTimeOnline.Core.Security
{
    public class RsaStore
    {
        public static readonly IList<RSACryptoServiceProvider> Store = new List<RSACryptoServiceProvider>();

        public static void Add(string keyContainerName, bool persistKeyInCsp = true)
        {
            if (string.IsNullOrEmpty(keyContainerName))
            {
                throw new ArgumentNullException("keyContainerName");
            }

            RSACryptoServiceProvider rsa =
                Store.FirstOrDefault(x => x.CspKeyContainerInfo.KeyContainerName == keyContainerName);

            if (rsa != null)
            {
                throw new Exception("There is already a RSACryptoServiceProvider with the same container name.");
            }


            rsa = new RSACryptoServiceProvider(2048, new CspParameters
            {
                KeyContainerName = keyContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            }) {PersistKeyInCsp = persistKeyInCsp};
            Store.Add(rsa);
        }

        public static void AddAndSave(string keyContainerName)
        {
            Add(keyContainerName);
            RSACryptoServiceProvider rsa = Get(keyContainerName);
            string xmlStringPrivateKey = rsa.ToXmlString(true);
            string xmlStringPublicKey = rsa.ToXmlString(false);
            string rsaPrivateKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                keyContainerName.ToLower().Replace("-", "_") + "-rsa-private-key.xml");
            string rsaPublicKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                keyContainerName.ToLower().Replace("-", "_") + "-rsa-public-key.xml");
            File.WriteAllText(rsaPrivateKeyPath, xmlStringPrivateKey);
            File.WriteAllText(rsaPublicKeyPath, xmlStringPublicKey);
        }

        public static RSACryptoServiceProvider Get(string keyContainerName)
        {
            RSACryptoServiceProvider rsa =
                Store.FirstOrDefault(x => x.CspKeyContainerInfo.KeyContainerName == keyContainerName);
            if (rsa == null)
            {
                throw new Exception("RSACryptoServiceProvider not found.");
            }
            return rsa;
        }

        public static RSACryptoServiceProvider Load(string keyContainerName)
        {
            RSACryptoServiceProvider rsa = Get(keyContainerName);
            if (rsa == null)
            {
                throw new Exception("RSACryptoServiceProvider not found.");
            }
            rsa.Clear();
            string rsaPrivateKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                keyContainerName.ToLower().Replace("-", "_") + "-rsa-private-key.xml");
            string xmlPrivateKey = File.ReadAllText(rsaPrivateKeyPath);
            rsa.FromXmlString(xmlPrivateKey);
            return rsa;
        }

        public static void Clear(string keyContainerName)
        {
            RSACryptoServiceProvider rsa = Get(keyContainerName);
            if (rsa == null)
            {
                throw new Exception("RSACryptoServiceProvider not found.");
            }
            rsa.Clear();
        }

        public static void Remove(string keyContainerName)
        {
            RSACryptoServiceProvider rsa = Get(keyContainerName);
            if (rsa == null)
            {
                throw new Exception("RSACryptoServiceProvider not found.");
            }
            rsa.Clear();
            if (rsa.PersistKeyInCsp)
            {
                rsa.PersistKeyInCsp = false;
            }
            Store.Remove(rsa);
        }
    }
}