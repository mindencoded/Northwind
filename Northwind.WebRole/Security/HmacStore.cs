using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Northwind.WebRole.Security
{
    public class HmacStore
    {
        public static readonly IDictionary<string, byte[]> Store = new ConcurrentDictionary<string, byte[]>();

        public static void Add(string key)
        {
            //string hmacSecretKey = Convert.ToBase64String(new HMACSHA256().Key);
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32];
                cryptoProvider.GetBytes(secretKeyByteArray);
                Store.Add(key, secretKeyByteArray);
            }
        }

        public static byte[] Get(string key)
        {
            if (!Store.ContainsKey(key))
            {
                return null;
            }

            return Store[key];
        }

        public static string ToBase64String(string key)
        {
            return Convert.ToBase64String(Store[key]);
        }

        public static void Remove(string key)
        {
            Store.Remove(key);
        }
    }
}