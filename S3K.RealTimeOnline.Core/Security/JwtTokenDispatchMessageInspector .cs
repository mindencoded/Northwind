using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using S3K.RealTimeOnline.CommonUtils;

namespace S3K.RealTimeOnline.Core.Security
{
    public class JwtTokenDispatchMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            object value = OperationContext.Current.IncomingMessageProperties.TryGetValue("Principal", out value)
                ? value
                : null;
            IPrincipal principal = value as IPrincipal;
            if (principal != null) return null;
            UriTemplateMatch uriTemplateMatch =
                (UriTemplateMatch) OperationContext.Current.IncomingMessageProperties["UriTemplateMatchResults"];
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
                        string xmlString = new FileAccessHelper(AppConfig.RsaPrivateKeyPath).Read();
                        isValid =
                            new JwtRsaValidator(xmlString, uriTemplateMatch.BaseUri.Host, uriTemplateMatch.BaseUri.Host)
                                .IsValid(encryptedToken, out claimsPrincipal);
                    }
                    else
                    {
                        string hmacSecretKey = AppConfig.HmacSecretKey;
                        isValid = new JwtHmacValidator(hmacSecretKey, uriTemplateMatch.BaseUri.Host,
                            uriTemplateMatch.BaseUri.Host).IsValid(encryptedToken, out claimsPrincipal);
                    }

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