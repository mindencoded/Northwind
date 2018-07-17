using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Northwind.WebRole.Security
{
    public class JwtRsaValidator
    {
        public static bool IsValid(RSACryptoServiceProvider rsaCryptoServiceProvider, string tokenString,
            string validAudience, string validIssuer)
        {
            return IsValid(rsaCryptoServiceProvider, tokenString, validAudience, validIssuer, out _);
        }

        public static bool IsValid(RSACryptoServiceProvider rsaCryptoServiceProvider, string tokenString,
            string validAudience, string validIssuer,
            out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaCryptoServiceProvider);
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidAudience = validAudience,
                    ValidIssuer = validIssuer,
                    IssuerSigningKey = rsaSecurityKey
                };
                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal =
                    securityTokenHandler.ValidateToken(tokenString, validationParameters, out securityToken);
                return securityToken != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}