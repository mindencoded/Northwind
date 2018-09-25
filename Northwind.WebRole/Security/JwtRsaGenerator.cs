using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Northwind.WebRole.Security
{
    public class JwtRsaGenerator
    {
        public static string Encode(RSACryptoServiceProvider cryptoServiceProvider, string name, string email,
            string[] roles, string audience, string issuer, double tokenExpirationMinutes = 30)
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

            SigningCredentials signingCredentials = new SigningCredentials(new RsaSecurityKey(cryptoServiceProvider),
                SecurityAlgorithms.RsaSha256Signature);
            JwtSecurityToken securityToken =
                new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow.AddSeconds(-1),
                    DateTime.UtcNow.AddMinutes(tokenExpirationMinutes), signingCredentials);
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.WriteToken(securityToken);
        }
    }
}