using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.GenericDataAccess.QueryHandlers;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core
{
    public abstract class BaseService
    {
        protected readonly IUnityContainer Container;

        protected BaseService(IUnityContainer container)
        {
            Container = container;
        }

        protected Stream Select<TUnitOfWork, TEntity>(string page, string pageSize)
            where TUnitOfWork : IUnitOfWork
            where TEntity : Entity
        {
            try
            {
                if (string.IsNullOrEmpty(page) || !Regex.IsMatch(page, @"\d") || Convert.ToInt32(page) == 0)
                {
                    throw new ValidationException("The 'page' parameter must be a valid number and greater than 0.");
                }

                if (string.IsNullOrEmpty(pageSize) || !Regex.IsMatch(pageSize, @"\d") || Convert.ToInt32(pageSize) == 0)
                {
                    throw new ValidationException(
                        "The 'pageSize' parameter must be a valid number and greater than 0.");
                }

                GenericSelectQuery query = new GenericSelectQuery
                {
                    Page = Convert.ToInt32(page),
                    PageSize = Convert.ToInt32(pageSize)
                };

                IGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>> handler =
                    ResolveGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>(
                        ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                        GenericQueryType.Select);
                IEnumerable<ExpandoObject> data = handler.Handle<TEntity>(query);
                string response = ResponseDataToString(data);
                return CreateStreamResponse(response);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        protected Stream SelectById<TUnitOfWork, TEntity>(string id)
            where TUnitOfWork : IUnitOfWork
            where TEntity : Entity
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Regex.IsMatch(id, @"\d") || Convert.ToInt32(id) == 0)
                {
                    throw new ValidationException("The 'id' parameter must be a valid number and cannot be zero.");
                }

                GenericSelectByIdQuery query = new GenericSelectByIdQuery
                {
                    Id = id
                };

                GenericSelectByIdQueryHandler<TUnitOfWork, TEntity> handler =
                    Container.Resolve<GenericSelectByIdQueryHandler<TUnitOfWork, TEntity>>();
                Entity entity = handler.Handle(query);
                string response = ResponseDataToString(entity);
                return CreateStreamResponse(response);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        protected void Insert<TUnitOfWork, TEntity, TDto>(TDto command)
            where TUnitOfWork : IUnitOfWork
            where TEntity : Entity
            where TDto : class
        {
            try
            {
                TEntity entity = new Mapper<TDto, TEntity>().CreateMappedObject(command);
                IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                    HandlerDecoratorType.ValidationCommand,
                    ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                    GenericCommandType.Insert);
                commandHandler.Handle<TEntity>(entity);
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        protected void Update<TUnitOfWork, TEntity, TDto>(string id, TDto command)
            where TUnitOfWork : IUnitOfWork
            where TEntity : Entity
            where TDto : class
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Regex.IsMatch(id, @"\d") || Convert.ToInt32(id) == 0)
                {
                    throw new ValidationException("The 'id' parameter must be a valid number and cannot be zero.");
                }

                GenericSelectByIdQuery query = new GenericSelectByIdQuery
                {
                    Id = id
                };

                TEntity entity = new Mapper<TDto, TEntity>().CreateMappedObject(command);
                GenericSelectByIdQueryHandler<TUnitOfWork, TEntity> queryHandler =
                    Container.Resolve<GenericSelectByIdQueryHandler<TUnitOfWork, TEntity>>();
                Entity result = queryHandler.Handle(query);
                if (result != null)
                {
                    IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                        HandlerDecoratorType.ValidationCommand,
                        ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                        GenericCommandType.Update);
                    commandHandler.Handle<TEntity>(entity);
                    if (WebOperationContext.Current != null)
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                }
                else
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        protected void PartialUpdate<TUnitOfWork, TEntity>(string id, string json)
            where TUnitOfWork : IUnitOfWork
            where TEntity : Entity
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Regex.IsMatch(id, @"\d") || Convert.ToInt32(id) == 0)
                {
                    throw new ValidationException("The 'id' parameter must be a valid number and cannot be zero.");
                }

                GenericSelectByIdQuery query = new GenericSelectByIdQuery
                {
                    Id = id
                };

                IDictionary<string, object> obj = DeserializeToDictionary<TEntity>(json);
                GenericSelectByIdQueryHandler<TUnitOfWork, TEntity> queryHandler =
                    Container.Resolve<GenericSelectByIdQueryHandler<TUnitOfWork, TEntity>>();
                Entity result = queryHandler.Handle(query);
                if (result != null)
                {
                    IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                        HandlerDecoratorType.ValidationCommand,
                        ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                        GenericCommandType.Update);
                    commandHandler.Handle<TEntity>(obj);
                    if (WebOperationContext.Current != null)
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                }
                else
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        protected void DeleteById<TUnitOfWork, TEntity>(string id)
            where TUnitOfWork : IUnitOfWork
            where TEntity : Entity
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Regex.IsMatch(id, @"\d") || Convert.ToInt32(id) == 0)
                {
                    throw new ValidationException("The 'id' parameter must be a valid number and cannot be zero.");
                }

                IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                    HandlerDecoratorType.TransactionCommand,
                    ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                    GenericCommandType.DeleteById);
                commandHandler.Handle<TEntity>(id);
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex),
                    ex is ValidationException
                        ? HttpStatusCode.BadRequest
                        : HttpStatusCode.InternalServerError);
            }
        }

        protected string ResponseDataToString(dynamic data)
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

        protected ExpandoObject DeserializeToExpandoObject<T>(string json) where T : class
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