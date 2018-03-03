using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.GenericDataAccess.QueryHandlers;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public abstract class CrudService<TUnitOfWork, TEntity, TDto> : BaseService
        where TUnitOfWork : IUnitOfWork
        where TEntity : Entity
        where TDto : class
    {
        protected CrudService(IUnityContainer container) : base(container)
        {
        }

        public virtual Stream Select(string page, string pageSize)
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

                GenericSelectQuery selectQuery = new GenericSelectQuery
                {
                    Page = Convert.ToInt32(page),
                    PageSize = Convert.ToInt32(pageSize)
                };

                GenericCountQuery countQuery = new GenericCountQuery
                {
                    Conditions = null
                };

                IGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>> selectQueryHandler =
                    ResolveGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>(
                        ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                        GenericQueryType.Select);

                IGenericQueryHandler<GenericCountQuery, int> countQueryHandler =
                    ResolveGenericQueryHandler<GenericCountQuery, int>(
                        ConfigContainer.UnitOfWorkDictionary.FirstOrDefault(x => x.Value == typeof(TUnitOfWork)).Key,
                        GenericQueryType.Count);

                IList<ExpandoObject> selectResult = selectQueryHandler.Handle<TEntity>(selectQuery).ToList();
                int countResult = countQueryHandler.Handle<TEntity>(countQuery);
                QueryResponse response = new QueryResponse
                {
                    Value = selectResult,
                    Total = countResult
                };
                string data = DataToString(response);
                return CreateStreamResponse(data);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        public Stream SelectById(string id)
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
                string response = DataToString(entity);
                return CreateStreamResponse(response);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }

        public void Insert(TDto dto)
        {
            try
            {
                ValidationHelper.ValidateObject(dto);
                TEntity entity = new Mapper<TDto, TEntity>().CreateMappedObject(dto);
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

        public void Update(string id, TDto dto)
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
                ValidationHelper.ValidateObject(dto);
                TEntity entity = new Mapper<TDto, TEntity>().CreateMappedObject(dto);
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

        public void PartialUpdate(string id, string json)
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
                ValidationHelper.ValidateProperties<TEntity>(obj);
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

        public void DeleteById(string id)
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
    }
}