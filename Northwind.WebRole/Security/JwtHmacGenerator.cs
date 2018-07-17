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

            DateTime expires = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes);
            claims.Add(new Claim(ClaimTypes.Expiration, expires.ToString("yyyyMMddHHmmss")));
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            // Build the token descriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                Subject = claimsIdentity,
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