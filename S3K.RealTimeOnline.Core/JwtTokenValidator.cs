using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace S3K.RealTimeOnline.Core
{
    public class JwtTokenValidator
    {
        private readonly string _privateKey;
        private readonly string _audience;
        private readonly string _issuer;

        public JwtTokenValidator(string privateKey, string audience, string issuer)
        {
            _privateKey = privateKey;
            _audience = audience;
            _issuer = issuer;
        }

        public bool Validate(string encryptedToken)
        {
            return Validate(encryptedToken, out _);
        }

        public bool Validate(string encryptedToken, out ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                claimsPrincipal = null;
                byte[] symmetricKey = Encoding.UTF8.GetBytes(_privateKey);
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

                return validatedToken != null;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), HttpStatusCode.Unauthorized);
            }
        }
    }
}