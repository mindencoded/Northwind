using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Northwind.WebRole.Security
{
    public class JwtHmacValidator
    {
        public static bool IsValid(string symmetricKey, string encryptedToken, string validAudience, string validIssuer)
        {
            return IsValid(Encoding.UTF8.GetBytes(symmetricKey), encryptedToken, validAudience, validIssuer, out _);
        }

        public static bool IsValid(string symmetricKey, string encryptedToken, string validAudience, string validIssuer,
            out ClaimsPrincipal claimsPrincipal)
        {
            return IsValid(Encoding.UTF8.GetBytes(symmetricKey), encryptedToken, validAudience, validIssuer,
                out claimsPrincipal);
        }

        public static bool IsValid(byte[] symmetricKey, string encryptedToken, string validAudience, string validIssuer)
        {
            return IsValid(symmetricKey, encryptedToken, validAudience, validIssuer, out _);
        }

        public static bool IsValid(byte[] symmetricKey, string encryptedToken, string validAudience, string validIssuer,
            out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = validAudience,
                    ValidIssuer = validIssuer,
                    IssuerSigningKey = signingKey
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal =
                    handler.ValidateToken(encryptedToken, tokenValidationParameters,
                        out securityToken);
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
            catch (Exception)
            {
                return false;
            }
        }
    }
}