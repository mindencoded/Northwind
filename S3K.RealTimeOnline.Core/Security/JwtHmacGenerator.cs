using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core.Security
{
    /// <summary>
    /// GenerateJwtToken class, build and creates the Jwt Token for the received parameters
    /// </summary>
    public class JwtHmacGenerator
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public JwtHmacGenerator(string privateKey, string audience, string issuer, double tokenExpirationMinutes)
        {
            _privateKey = privateKey;
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        /// <summary>
        /// Create the token handler and build the token with claims.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="email">Email</param>
        /// <param name="roles">Roles</param>
        /// <returns></returns>
        public string Encode(string name, string email, string[] roles)
        {
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

            // Build the symmetric key.      
            byte[] symmetricKey = Encoding.UTF8.GetBytes(_privateKey);
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
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
                SigningCredentials = signingCredentials
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