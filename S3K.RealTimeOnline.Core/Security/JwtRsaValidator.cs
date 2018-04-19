using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtRsaValidator
    {
        public static bool IsValid(RSACryptoServiceProvider rsaCryptoServiceProvider, string tokenString)
        {
            return IsValid(rsaCryptoServiceProvider, tokenString, out _);
        }

        public static bool IsValid(RSACryptoServiceProvider rsaCryptoServiceProvider, string tokenString,
            out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaCryptoServiceProvider);
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
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