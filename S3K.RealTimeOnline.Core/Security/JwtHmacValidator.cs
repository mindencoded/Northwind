using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtHmacValidator
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;

        public JwtHmacValidator(string privateKey, string audience, string issuer)
        {
            _privateKey = privateKey;
            _audience = audience;
            _issuer = issuer;
        }

        public bool IsValid(string encryptedToken)
        {
            return IsValid(encryptedToken, out _);
        }

        public bool IsValid(string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                byte[] symmetricKey = Encoding.UTF8.GetBytes(_privateKey);
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