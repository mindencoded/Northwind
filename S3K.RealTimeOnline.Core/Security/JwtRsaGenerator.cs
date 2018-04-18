using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtRsaGenerator
    {
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public JwtRsaGenerator(string audience, string issuer)
        {
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = 30;
        }

        public JwtRsaGenerator(string audience, string issuer, double tokenExpirationMinutes)
        {
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        public string Encode(RSACryptoServiceProvider cryptoServiceProvider, string name, string email, string[] roles)
        {
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name)
            };

            if (!string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            DateTime expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes);
            claims.Add(new Claim(ClaimTypes.Expiration, expires.ToString("yyyyMMddHHmmss")));
            JwtSecurityToken securityToken = new JwtSecurityToken
            (
                audience: _audience,
                issuer: _issuer,
                claims: claims,
                signingCredentials: new SigningCredentials(new RsaSecurityKey(cryptoServiceProvider),
                    SecurityAlgorithms.RsaSha256Signature),
                expires: expires,
                notBefore: DateTime.UtcNow
            );

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.WriteToken(securityToken);
        }
    }
}