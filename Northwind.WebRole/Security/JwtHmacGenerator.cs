using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Northwind.WebRole.Security
{
    public class JwtHmacGenerator
    {
        public static string Encode(string symmetricKey, string name, string email, string[] roles, string audience,
            string issuer,
            double tokenExpirationMinutes = 30)
        {
            return Encode(Encoding.UTF8.GetBytes(symmetricKey), name, email, roles, audience, issuer,
                tokenExpirationMinutes);
        }

        public static string Encode(byte[] symmetricKey, string name, string email, string[] roles, string audience,
            string issuer,
            double tokenExpirationMinutes = 30)
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

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                SigningCredentials = signingCredentials,
                NotBefore = DateTime.UtcNow.AddSeconds(-1)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(securityTokenDescriptor);
            return handler.WriteToken(securityToken);
        }
    }
}