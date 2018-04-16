using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtRsaGenerator
    {
        private readonly string _xmlString;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public JwtRsaGenerator(string xmlString, string audience, string issuer, double tokenExpirationMinutes)
        {
            _xmlString = xmlString;
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        public string Encode(string name, string email, string[] roles, string keyContainerName = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            RSACryptoServiceProvider
                cryptoServiceProvider;
            if (keyContainerName != null)
            {
                cryptoServiceProvider = new RSACryptoServiceProvider(2048, new CspParameters
                {
                    KeyContainerName = keyContainerName
                });
            }
            else
            {
                cryptoServiceProvider = new RSACryptoServiceProvider(2048);
            }
            cryptoServiceProvider.FromXmlString(_xmlString);

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name)
            };

            if (!string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken securityToken = new JwtSecurityToken
            (
                audience: _audience,
                issuer: _issuer,
                claims: claims,
                signingCredentials: new SigningCredentials(new RsaSecurityKey(cryptoServiceProvider),
                    SecurityAlgorithms.RsaSha256Signature),
                expires: DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
                notBefore: DateTime.UtcNow
            );

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.WriteToken(securityToken);
        }
    }
}