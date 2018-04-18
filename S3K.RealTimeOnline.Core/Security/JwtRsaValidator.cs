using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtRsaValidator
    {
        private readonly string _audience;
        private readonly string _issuer;

        public JwtRsaValidator(string audience, string issuer)
        {
            _audience = audience;
            _issuer = issuer;
        }

        public bool IsValid(RSACryptoServiceProvider rsaCryptoServiceProvider, string tokenString)
        {
            return IsValid(rsaCryptoServiceProvider, tokenString, out _);
        }

        public bool IsValid(RSACryptoServiceProvider rsaCryptoServiceProvider, string tokenString,
            out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaCryptoServiceProvider);
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