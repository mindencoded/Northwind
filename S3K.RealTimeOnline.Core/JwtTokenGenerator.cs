using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using SecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;

namespace S3K.RealTimeOnline.Core
{
    /// <summary>
    /// GenerateJwtToken class, build and creates the Jwt Token for the received parameters
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public JwtTokenGenerator(string privateKey, string audience, string issuer, double tokenExpirationMinutes)
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
            // Create the handler
            var handler = new JwtSecurityTokenHandler();
            // Build the token
            var token = CreateJwtSecurityToken(name, email, roles);
            // Return the token to Encode method
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Build the Token body with email, name and roles.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="email">Email</param>
        /// <param name="roles">Roles</param>
        /// <returns></returns>
        public JwtSecurityToken CreateJwtSecurityToken(string name, string email, string[] roles)
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
            byte[] symmetricKey = Encoding.UTF8.GetBytes(_privateKey); //GetBytes(_privateKey);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);

            // Build the token descriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _audience,
                Issuer = _issuer,
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the security handler to call    
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            // Create JWT token.
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(securityTokenDescriptor);
            // Return the token to Encode function call, which in turn return to Main function.
            return securityToken;
        }
    }
}