using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtHmacValidator
    {
        private readonly string _audience;
        private readonly string _issuer;

        public JwtHmacValidator(string audience, string issuer)
        {
            _audience = audience;
            _issuer = issuer;
        }

        public bool IsValid(string symmetricKey, string encryptedToken)
        {
            return IsValid(Encoding.UTF8.GetBytes(symmetricKey), encryptedToken, out _);
        }

        public bool IsValid(string symmetricKey, string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            return IsValid(Encoding.UTF8.GetBytes(symmetricKey), encryptedToken, out claimsPrincipal);
        }

        public bool IsValid(byte[] symmetricKey, string encryptedToken)
        {
            return IsValid(symmetricKey, encryptedToken, out _);
        }

        public bool IsValid(byte[] symmetricKey, string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudiences = new[]
                    {
                        _audience
                    },
                    ValidIssuers = new[]
                    {
                        _issuer
                    },
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