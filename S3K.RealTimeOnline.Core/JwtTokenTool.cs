using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RsaSecurityKey = Microsoft.IdentityModel.Tokens.RsaSecurityKey;
using SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using SecurityToken = Microsoft.IdentityModel.Tokens.SecurityToken;
using SecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;

namespace S3K.RealTimeOnline.Core
{
    public class JwtTokenTool
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public JwtTokenTool(string privateKey, string audience, string issuer, double tokenExpirationMinutes)
        {
            _privateKey = privateKey;
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        public string Create(string name, string email, string[] roles)
        {
            JwtSecurityTokenHandler tokenHandler;
            JwtSecurityToken jwtSecurityToken = Create(name, email, roles, out tokenHandler);
            return tokenHandler.WriteToken(jwtSecurityToken);
        }

        public JwtSecurityToken Create(string name, string email, string[] roles,
            out JwtSecurityTokenHandler tokenHandler)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_privateKey));
            SigningCredentials signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);


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

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Custom");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _audience,
                Issuer = _issuer,
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.UtcNow,
                EncryptingCredentials = new EncryptingCredentials(signingKey, JwtConstants.DirectKeyUseAlg,
                    SecurityAlgorithms.Aes256CbcHmacSha512)
            };

            tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
        }

        public bool Validate(string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;

            try
            {          
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_privateKey));

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
                    IssuerSigningKey = signingKey
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal = tokenHandler.ValidateToken(encryptedToken, tokenValidationParameters, out securityToken);
                return securityToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string CreateRsaJwtSecurityToken(string name, string email, string[] roles)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            /*
             SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_privateKey));
            SigningCredentials signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
             */

            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.FromXmlString(_privateKey);
            RsaSecurityKey signingKey = new RsaSecurityKey(cryptoServiceProvider);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);
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

            SecurityToken securityToken = new JwtSecurityToken
            (
                audience: _audience,
                issuer: _issuer,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(_tokenExpirationMinutes)
            );

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.WriteToken(securityToken);
        }

        public bool ValidateRsaJwtSecurityToken(string tokenString, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            try
            {               
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                cryptoServiceProvider.FromXmlString(_privateKey);
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(cryptoServiceProvider);
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = rsaSecurityKey
                };
                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                claimsPrincipal = securityTokenHandler.ValidateToken(tokenString, validationParameters, out securityToken);
                return securityToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}