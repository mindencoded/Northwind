using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using S3K.RealTimeOnline.CommonUtils;
using RsaSecurityKey = Microsoft.IdentityModel.Tokens.RsaSecurityKey;
using SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using SecurityToken = Microsoft.IdentityModel.Tokens.SecurityToken;
using SecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;

namespace S3K.RealTimeOnline.Core
{
    public class TokenCreator
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly double _tokenExpirationMinutes;

        public TokenCreator()
        {
            _privateKey = AppConfig.PrivateKey;
            _audience = AppConfig.Audience;
            _issuer = AppConfig.Issuer;
            _tokenExpirationMinutes = AppConfig.TokenExpirationMinutes;
        }

        public TokenCreator(string privateKey, string audience, string issuer, double tokenExpirationMinutes)
        {
            _privateKey = privateKey;
            _audience = audience;
            _issuer = issuer;
            _tokenExpirationMinutes = tokenExpirationMinutes;
        }

        public string CreateJwtSecurityToken(string name, string email, string[] roles)
        {
            SecurityTokenDescriptor securityTokenDescriptor = CreateSecurityTokenDescriptor(name, email, roles);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken plainToken = tokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(plainToken);
        }

        public string CreateToken(string name, string email, string[] roles)
        {
            SecurityTokenDescriptor securityTokenDescriptor = CreateSecurityTokenDescriptor(name, email, roles);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(plainToken);
        }

        public SecurityTokenDescriptor CreateSecurityTokenDescriptor(string name, string email, string[] roles)
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
                new Claim(ClaimTypes.NameIdentifier, name)
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
                //IssuedAt = DateTime.UtcNow,
                //EncryptingCredentials = new EncryptingCredentials(signingKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512)
            };

            return securityTokenDescriptor;
        }

        public string GenerateRsaToken(string name, string email, string[] roles)
        {
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, name),
                new Claim(ClaimTypes.Email, email)
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.FromXmlString(_privateKey);

            JwtSecurityToken securityToken = new JwtSecurityToken
            (
                _audience,
                _issuer,
                claims,
                signingCredentials: new SigningCredentials(new RsaSecurityKey(cryptoServiceProvider),
                    SecurityAlgorithms.RsaSha256Signature),
                expires: DateTime.Now.AddDays(_tokenExpirationMinutes)
            );

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.WriteToken(securityToken);
        }

        public bool ValidateToken(string tokenString)
        {
            try
            {
                SecurityToken securityToken = new JwtSecurityToken(tokenString);
                JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
                RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
                publicAndPrivate.FromXmlString(_privateKey);
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new RsaSecurityKey(publicAndPrivate)
                };
                SecurityToken outSecurityToken;
                securityTokenHandler.ValidateToken(tokenString, validationParameters, out outSecurityToken);

                Debug.WriteLine(@"securityToken : {0}", securityToken);
                Debug.WriteLine(@"outsecurityToken : {0}", outSecurityToken);
                if (outSecurityToken != null) return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            return false;
        }

        public string GenerateToken(string username, int expireMinutes = 20)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec =
                "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            SigningCredentials signingCredentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            JwtSecurityToken token =
                tokenHandler.CreateJwtSecurityToken(_issuer, _audience,
                    claimsIdentity, issuedAt: issuedAt, expires: expires, signingCredentials: signingCredentials);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public bool ValidateReceivedToken(string encryptedToken)
        {
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
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(encryptedToken, tokenValidationParameters, out validatedToken);
                return validatedToken != null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            return false;
        }
    }
}