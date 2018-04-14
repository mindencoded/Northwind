using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using S3K.RealTimeOnline.CommonUtils;

namespace S3K.RealTimeOnline.Core
{
    public class JwtTokenValidator
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;

        public JwtTokenValidator()
        {
            _privateKey = AppConfig.PrivateKey;
            _audience = AppConfig.Audience;
            _issuer = AppConfig.Issuer;
        }

        public bool Validate(string encryptedToken)
        {
            return Validate(encryptedToken, out _);
        }

        public bool Validate(string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            byte[] symmetricKey = Encoding.UTF8.GetBytes(_privateKey); //GetBytes(_privateKey);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(symmetricKey);
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

            SecurityToken validatedToken;

            claimsPrincipal =
                new JwtSecurityTokenHandler().ValidateToken(encryptedToken, tokenValidationParameters,
                    out validatedToken);

            if (validatedToken != null)
            {
                return true;
            }
            return false;
        }

        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}