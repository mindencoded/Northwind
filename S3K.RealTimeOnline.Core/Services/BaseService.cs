using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public abstract class BaseService
    {
        protected readonly IUnityContainer Container;

        protected BaseService(IUnityContainer container)
        {
            Container = container;
        }

        protected string DataToString(dynamic data)
        {
            JsonSerializer s = JsonSerializer.Create();
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s.Serialize(sw, data);
            }

            return sb.ToString();
        }

        protected Stream CreateStreamResponse(string response)
        {
            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.ContentType =
                    "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }

        protected Message CreateExceptionMessage<T>(T error, WebContentFormat format = WebContentFormat.Json)
        {
            Message message = Message.CreateMessage(MessageVersion.None, "", error,
                new DataContractJsonSerializer(typeof(T)));
            message.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(format));
            return message;
        }

        protected Stream CreateStreamResponse(string response, HttpStatusCode statusCode,
            string contentType = "application/json; charset=utf-8")
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = contentType;
                WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }

        protected IGenericCommandHandler ResolveGenericCommandHandler(UnitOfWorkType unitOfWorkType,
            GenericCommandType genericCommandType)
        {
            return Container.Resolve<IGenericCommandHandler>(unitOfWorkType + "_" + genericCommandType);
        }

        protected IGenericCommandHandler ResolveGenericCommandHandler(HandlerDecoratorType handlerDecoratorType,
            UnitOfWorkType unitOfWorkType, GenericCommandType genericCommandType)
        {
            return Container.Resolve<IGenericCommandHandler>(
                handlerDecoratorType + "_" + unitOfWorkType + "_" + genericCommandType);
        }

        protected IGenericQueryHandler<TQuery, TResult> ResolveGenericQueryHandler<TQuery, TResult>(
            UnitOfWorkType unitOfWorkType, GenericQueryType genericQueryType)
            where TQuery : IQuery<TResult>
        {
            return Container.Resolve<IGenericQueryHandler<TQuery, TResult>>(unitOfWorkType + "_" + genericQueryType);
        }

        protected dynamic DeserializeToDynamic(string json)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        }

        protected T DeserializeToObject<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected ExpandoObject DeserializeToExpando<T>(string json) where T : class
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

        protected IDictionary<string, object> DeserializeToDictionary<T>(string json) where T : class
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