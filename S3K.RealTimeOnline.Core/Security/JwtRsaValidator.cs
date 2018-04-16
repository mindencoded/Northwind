using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtRsaValidator
    {
        private readonly string _xmlString;
        private readonly string _audience;
        private readonly string _issuer;

        public JwtRsaValidator(string xmlString, string audience, string issuer)
        {
            _xmlString = xmlString;
            _audience = audience;
            _issuer = issuer;
        }

        public bool IsValid(string tokenString, string keyContainerName = null)
        {
            return IsValid(tokenString, out _, keyContainerName);
        }

        public bool IsValid(string tokenString, out ClaimsPrincipal claimsPrincipal, string keyContainerName = null)
        {
            claimsPrincipal = null;
            try
            {
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
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(cryptoServiceProvider);
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = rsaSecurityKey
                };
                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal =
                    securityTokenHandler.ValidateToken(tokenString, validationParameters, out securityToken);
                return securityToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}