using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Northwind.Shared;

namespace Northwind.WebRole.Security
{
    public class JwtTokenDispatchMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            UriTemplateMatch uriTemplateMatch =
                (UriTemplateMatch) OperationContext.Current.IncomingMessageProperties["UriTemplateMatchResults"];
            string host = uriTemplateMatch.BaseUri.Host;
            object value = OperationContext.Current.IncomingMessageProperties.TryGetValue("Principal", out value)
                ? value
                : null;
            IPrincipal principal = value as IPrincipal;
            if (principal != null) return null;
            if (ContextHelper.GetRoleName(OperationContext.Current) != null)
            {
                HttpRequestMessageProperty httpRequest = (HttpRequestMessageProperty)
                    OperationContext.Current.IncomingMessageProperties["httpRequest"];
                string encryptedToken = httpRequest.Headers["JWTTOKEN"];
                if (!string.IsNullOrEmpty(encryptedToken))
                {
                    ClaimsPrincipal claimsPrincipal;
                    bool isValid;
                    if (AppConfig.UseRsa)
                    {
                        RSACryptoServiceProvider rsa = RsaStore.Get("Custom");
                        isValid = JwtRsaValidator.IsValid(rsa, encryptedToken, host, host, out claimsPrincipal);
                    }
                    else
                    {
                        byte[] symmetricKey = HmacStore.Get("Custom");
                        isValid = JwtHmacValidator.IsValid(symmetricKey, encryptedToken, host, host,
                            out claimsPrincipal);
                    }

                    if (isValid)
                    {
                        Claim expirationClaim =
                            claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration);
                        if (expirationClaim != null)
                        {
                            DateTime expires = DateTime.ParseExact(expirationClaim.Value, "yyyyMMddHHmmss",
                                CultureInfo.InvariantCulture);
                            isValid = DateTime.Compare(expires, DateTime.Now) > 0;
                            if (isValid)
                            {
                                Claim nameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                                if (nameClaim != null)
                                {
                                    string[] roles = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role)
                                        .Select(c => c.Value).ToArray();
                                    principal = new CustomPrincipal(new GenericIdentity(nameClaim.Value), roles);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                principal = new CustomPrincipal(new GenericIdentity("Anonymous"), new string[] { });
            }

            if (principal != null)
            {
                OperationContext.Current.IncomingMessageProperties.Add("Principal", principal);
            }

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}