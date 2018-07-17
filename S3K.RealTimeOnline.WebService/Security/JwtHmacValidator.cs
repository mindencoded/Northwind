using System;
using System.Security.Claims;
using System.Text;

namespace S3K.RealTimeOnline.WebService.Security
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

                return securityToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}