using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtHmacGenerator
    {
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public JwtHmacGenerator(string audience, string issuer)
        {
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = 30;
        }

        public JwtHmacGenerator(string audience, string issuer, double tokenExpirationMinutes)
        {
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        public string Encode(string symmetricKey, string name, string email, string[] roles)
        {
            return Encode(Encoding.UTF8.GetBytes(symmetricKey), name, email, roles);
        }

        public string Encode(byte[] symmetricKey, string name, string email, string[] roles)
        {
            if (symmetricKey == null)
            {
                throw new ArgumentNullException("symmetricKey");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

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
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            // Build the token descriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Audience = _audience,
                Issuer = _issuer,
                Expires = expires,
                SigningCredentials = signingCredentials,
                NotBefore = DateTime.UtcNow
            };
            // Create the security handler to call    
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // Create JWT token.
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(securityTokenDescriptor);
            // Return the token to Encode function call, which in turn return to Main function.

            // Return the token to Encode method
            return handler.WriteToken(securityToken);
        }
    }
}