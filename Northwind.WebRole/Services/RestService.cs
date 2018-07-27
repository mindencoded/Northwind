using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Policy;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Northwind.WebRole.Decorators;
using Northwind.WebRole.Queries;
using Northwind.WebRole.Security;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;
using Unity;

namespace Northwind.WebRole.Services
{
    public abstract class RestService
    {
        protected readonly IUnityContainer Container;

        protected RestService(IUnityContainer container)
        {
            Container = container;
        }

        protected JsonSerializerSettings JsonSerializerSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented
                };
            }
        }

        protected static void WebHttpConfigure<TService>(ServiceConfiguration config, string address)
        {
            config.Description.Behaviors.Add(new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpsGetEnabled = true
            });

            config.Description.Behaviors.Remove<ServiceDebugBehavior>();
            config.Description.Behaviors.Add(new ServiceDebugBehavior
            {
                IncludeExceptionDetailInFaults = true
            });

            WebHttpBinding webHttpBinding = new WebHttpBinding
            {
                MaxBufferPoolSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647
            };

            WebHttpSecurity webHttpSecurity = new WebHttpSecurity
            {
                Mode = WebHttpSecurityMode.None,
                Transport = new HttpTransportSecurity
                {
                    ClientCredentialType = HttpClientCredentialType.None
                }
            };

            if (AppConfig.SslFlags.Contains(SslFlag.Ssl) || AppConfig.SslFlags.Contains(SslFlag.SslRequireCert) ||
                AppConfig.SslFlags.Contains(SslFlag.SslNegotiateCert))
            {
                webHttpSecurity.Mode = WebHttpSecurityMode.Transport;
            }

            if (AppConfig.SslFlags.Contains(SslFlag.SslRequireCert) ||
                AppConfig.SslFlags.Contains(SslFlag.SslNegotiateCert))
            {
                webHttpSecurity.Transport = new HttpTransportSecurity
                {
                    ClientCredentialType = HttpClientCredentialType.Certificate
                };

                config.Credentials.ClientCertificate.SetCertificate(
                    AppConfig.StoreLocation,
                    AppConfig.StoreName,
                    AppConfig.X509FindType,
                    AppConfig.FindValue);
            }

            webHttpBinding.Security = webHttpSecurity;
            ServiceEndpoint serviceEndpoint = config.AddServiceEndpoint(typeof(TService), webHttpBinding, address);
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior
            {
                DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                AutomaticFormatSelectionEnabled = false
            });

            config.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            if (AppConfig.EnableSecurity)
            {
                serviceEndpoint.EndpointBehaviors.Add(new JwtTokenBehaviorExtension());
                config.Authorization.ServiceAuthorizationManager =
                    new CustomAuthorizationManager(new ConfigContainer().Instance());
            }
            else
            {
                config.Authorization.ServiceAuthorizationManager = new AnonymousAuthorizationManager();
            }

            config.Authorization.ExternalAuthorizationPolicies =
                new List<IAuthorizationPolicy> {new CustomAuthorizationPolicy()}.AsReadOnly();
        }

        protected virtual string DataToString(dynamic data)
        {
            JsonSerializer s = JsonSerializer.Create();
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s.Serialize(sw, data);
            }

            return sb.ToString();
        }

        protected virtual Stream CreateStreamResponse(string response)
        {
            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.ContentType =
                    "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }

        public static Message CreateExceptionMessage<T>(T error, WebContentFormat format = WebContentFormat.Json)
        {
            Message message = Message.CreateMessage(MessageVersion.None, "", error,
                new DataContractJsonSerializer(typeof(T)));
            message.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(format));
            return message;
        }

        protected virtual Stream CreateStreamResponse(string response, HttpStatusCode statusCode,
            string contentType = "application/json; charset=utf-8")
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = contentType;
                WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }

        protected Message CreateResponseMessage(object message, string contentType = "application/json; charset=utf-8")
        {
            string myResponseBody = JsonConvert.SerializeObject(message, Formatting.None, JsonSerializerSettings);
            return WebOperationContext.Current != null
                ? WebOperationContext.Current.CreateTextResponse(myResponseBody,
                    contentType,
                    Encoding.UTF8)
                : null;
        }

        protected Message CreateJsonStream(object obj)
        {
            string jsonSerialized = JsonConvert.SerializeObject(obj, Formatting.None, JsonSerializerSettings);
            MemoryStream memoryStream = new MemoryStream(new UTF8Encoding().GetBytes(jsonSerialized)) {Position = 0};
            return WebOperationContext.Current != null
                ? WebOperationContext.Current.CreateStreamResponse(memoryStream, "application/json; charset=utf-8")
                : null;
        }

        protected virtual IGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>
            InstanceSelectQueryHandler<TUnitOfWork>()
        {
            return
                ResolveGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>(
                    ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                    GenericQueryType.Select);
        }

        protected virtual IGenericQueryHandler<GenericCountQuery, int> InstanceCountQueryHandler<TUnitOfWork>()
        {
            return
                ResolveGenericQueryHandler<GenericCountQuery, int>(
                    ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                    GenericQueryType.Count);
        }

        protected virtual IGenericCommandHandler ResolveGenericCommandHandler(UnitOfWorkType unitOfWorkType,
            GenericCommandType genericCommandType)
        {
            return Container.Resolve<IGenericCommandHandler>(unitOfWorkType + "_" + genericCommandType);
        }

        protected virtual IGenericCommandHandler ResolveGenericCommandHandler(HandlerDecoratorType handlerDecoratorType,
            UnitOfWorkType unitOfWorkType, GenericCommandType genericCommandType)
        {
            return Container.Resolve<IGenericCommandHandler>(
                handlerDecoratorType + "_" + unitOfWorkType + "_" + genericCommandType);
        }

        protected virtual IGenericQueryHandler<TQuery, TResult> ResolveGenericQueryHandler<TQuery, TResult>(
            UnitOfWorkType unitOfWorkType, GenericQueryType genericQueryType)
            where TQuery : IQuery<TResult>
        {
            return Container.Resolve<IGenericQueryHandler<TQuery, TResult>>(unitOfWorkType + "_" + genericQueryType);
        }

        protected virtual dynamic DeserializeToDynamic(string json)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        }

        protected virtual T DeserializeToObject<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected virtual ExpandoObject DeserializeToExpando<T>(string json) where T : class
        {
            JObject jObj = JObject.Parse(json);
            ExpandoObject expandoObject = new ExpandoObject();
            ICollection<KeyValuePair<string, object>> keyValuePairs = expandoObject;
            foreach (JProperty jProp in jObj.Properties())
            {
                string name = jProp.Name;
                if (!typeof(T).HasProperty(name)) continue;
                PropertyInfo property = typeof(T).GetProperty(name);
                if (property == null) continue;
                object value = Convert.ChangeType(jProp.Value, property.PropertyType);
                KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(name, value);
                keyValuePairs.Add(keyValuePair);
            }

            expandoObject = keyValuePairs as ExpandoObject;
            return expandoObject;
        }

        protected virtual IDictionary<string, object> DeserializeToDictionary<T>(string json) where T : class
        {
            JObject jObj = JObject.Parse(json);
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (JProperty jProp in jObj.Properties())
            {
                string name = jProp.Name;
                if (!typeof(T).HasProperty(name)) continue;
                PropertyInfo property = typeof(T).GetProperty(name);
                if (property == null) continue;
                object value = Convert.ChangeType(jProp.Value, property.PropertyType);
                KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(name, value);
                dictionary.Add(keyValuePair);
            }

            return dictionary;
        }
    }
}