using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtHmacValidator
    {
        public static bool IsValid(string symmetricKey, string encryptedToken)
        {
            return IsValid(Encoding.UTF8.GetBytes(symmetricKey), encryptedToken, out _);
        }

        public static bool IsValid(string symmetricKey, string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            return IsValid(Encoding.UTF8.GetBytes(symmetricKey), encryptedToken, out claimsPrincipal);
        }

        public static bool IsValid(byte[] symmetricKey, string encryptedToken)
        {
            return IsValid(symmetricKey, encryptedToken, out _);
        }

        public static bool IsValid(byte[] symmetricKey, string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal =
                    handler.ValidateToken(encryptedToken, tokenValidationParameters,
                        out securityToken);

                return securityToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}