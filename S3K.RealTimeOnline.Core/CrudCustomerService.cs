using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using S3K.RealTimeOnline.BusinessDataAccess.CommandHandlers.MoveCustomer;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.GenericDataAccess.QueryHandlers;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core
{
    public partial class MainService : ICrudCustomerService
    {
        public Stream SelectCustomer(string page, string pageSize)
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
                    ResolveGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>(UnitOfWorkType.Business,
                        GenericQueryType.Select);
                IEnumerable<ExpandoObject> data = handler.Handle<Customer>(query);
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

        public Stream SelectCustomerById(string id)
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

                GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer> handler =
                    Container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
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

        public void InsertCustomer(CustomerDto command)
        {
            try
            {
                Customer customer = new Mapper<CustomerDto, Customer>().CreateMappedObject(command);
                IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                    HandlerDecoratorType.ValidationCommand,
                    UnitOfWorkType.Business, GenericCommandType.Insert);
                commandHandler.Handle<Customer>(customer);
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

        public void UpdateCustomer(string id, CustomerDto command)
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

                Customer customer = new Mapper<CustomerDto, Customer>().CreateMappedObject(command);
                GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer> queryHandler =
                    Container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
                Entity result = queryHandler.Handle(query);
                if (result != null)
                {
                    if (customer.Id != int.Parse(id)) customer.Id = Convert.ToInt32(id);
                    IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                        HandlerDecoratorType.ValidationCommand,
                        UnitOfWorkType.Business, GenericCommandType.Update);
                    commandHandler.Handle<Customer>(customer);
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

        public void PartialUpdateCustomer(string id, string json)
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

                IDictionary<string, object> obj = DeserializeToDictionary<Customer>(json);
                GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer> queryHandler =
                    Container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
                Entity result = queryHandler.Handle(query);
                if (result != null)
                {
                    if (!obj.ContainsKey("Id") || obj["Id"].ToString() != id)
                    {
                        obj["Id"] = Convert.ToInt32(id);
                    }

                    IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                        HandlerDecoratorType.ValidationCommand,
                        UnitOfWorkType.Business, GenericCommandType.Update);
                    commandHandler.Handle<Customer>(obj);
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

        public void DeleteCustomerById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Regex.IsMatch(id, @"\d") || Convert.ToInt32(id) == 0)
                {
                    throw new ValidationException("The 'id' parameter must be a valid number and cannot be zero.");
                }

                IGenericCommandHandler commandHandler = ResolveGenericCommandHandler(
                    HandlerDecoratorType.TransactionCommand,
                    UnitOfWorkType.Business, GenericCommandType.DeleteById);
                commandHandler.Handle<Customer>(id);
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

        public void MoveCustomer()
        {
            try
            {
                MoveCustomerCommand command = new MoveCustomerCommand
                {
                    CustomerId = 1,
                    NewAddress = new Address
                    {
                        Description = "Address"
                    }
                };
                ICommandHandler<MoveCustomerCommand> handler =
                    Container.Resolve<ICommandHandler<MoveCustomerCommand>>(
                        HandlerDecoratorType.ValidationCommand.ToString());
                handler.Handle(command);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), HttpStatusCode.InternalServerError);
            }
        }
    }
}