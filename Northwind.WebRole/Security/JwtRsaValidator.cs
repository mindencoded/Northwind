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
                    IssuerSigningKey = rsaSecurityKey,
                    RequireExpirationTime = true
                };
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal =
                    jwtSecurityTokenHandler.ValidateToken(tokenString, validationParameters, out securityToken);
                JwtSecurityToken jwtSecurityToken = (JwtSecurityToken) securityToken;
                JwtPayload jwtPayload = jwtSecurityToken.Payload;
                int exp = Convert.ToInt32(jwtPayload.Exp);
                TimeSpan expTime = TimeSpan.FromSeconds(exp);
                DateTime expDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).Add(expTime);
                if (expDate < DateTime.UtcNow)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}