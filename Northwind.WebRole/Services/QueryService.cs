using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Queries;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;
using Unity;

namespace Northwind.WebRole.Services
{
    public abstract class QueryService<TUnitOfWork, TEntity> : WebHttpService
        where TUnitOfWork : IUnitOfWork
        where TEntity : Entity
    {
        protected QueryService(IUnityContainer container) : base(container)
        {
        }

        public virtual Stream Select(string page, string pageSize, string orderBy, string filter)
        {
            return Select(page, pageSize, orderBy, filter, null);
        }

        public virtual Stream Select(string page, string pageSize, string orderBy, string filter, string select)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(page) || !Regex.IsMatch(page, @"\d") || Convert.ToInt32(page) == 0)
                {
                    throw new ValidationException("The 'page' parameter must be a valid number and greater than 0.");
                }

                if (string.IsNullOrWhiteSpace(pageSize) || !Regex.IsMatch(pageSize, @"\d") ||
                    Convert.ToInt32(pageSize) == 0)
                {
                    throw new ValidationException(
                        "The 'pageSize' parameter must be a valid number and greater than 0.");
                }


                GenericSelectQuery selectQuery = new GenericSelectQuery
                {
                    Page = Convert.ToInt32(page),
                    PageSize = Convert.ToInt32(pageSize)
                };

                GenericCountQuery countQuery = new GenericCountQuery();

                if (!string.IsNullOrWhiteSpace(orderBy) && orderBy != "null")
                {
                    selectQuery.OrderBy = orderBy; //QueryHelper.CreateOrderByString<TEntity>(orderby.Split(','));
                }

                if (!string.IsNullOrWhiteSpace(filter) && filter != "null")
                {
                    IList<ParameterBuilder> conditions = new List<ParameterBuilder>();
                    string[] andConditions = filter.Split(new[] { " and ", " AND ", "And" },
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (string andCondition in andConditions)
                    {
                        string[] orConditions =
                            andCondition.Split(new[] { " or ", " OR ", "Or" }, StringSplitOptions.RemoveEmptyEntries);
                        if (orConditions.Length > 1)
                        {
                            foreach (string orCondition in orConditions)
                            {
                                ParameterBuilder parameterBuilder = ParameterBuilder.Create(orCondition, Condition.Or);
                                if (parameterBuilder != null)
                                {
                                    conditions.Add(parameterBuilder);
                                }
                            }
                        }
                        else
                        {
                            ParameterBuilder parameterBuilder = ParameterBuilder.Create(andCondition, Condition.And);
                            if (parameterBuilder != null)
                            {
                                conditions.Add(parameterBuilder);
                            }
                        }
                    }

                    if (conditions.Any())
                    {
                        selectQuery.Conditions = conditions;
                        countQuery.Conditions = conditions;
                    }
                }

                if (!string.IsNullOrWhiteSpace(select) && select != "null")
                {
                    string[] columns = select.Split(',');
                    if (columns.Any())
                    {
                        //ValidationHelper.ValidateProperties<TEntity>(columns);
                        selectQuery.Columns = columns;
                    }
                }

                IGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>> selectQueryHandler =
                    InstanceSelectQueryHandler<TUnitOfWork>();

                IGenericQueryHandler<GenericCountQuery, int>
                    countQueryHandler = InstanceCountQueryHandler<TUnitOfWork>();

                IList<ExpandoObject> selectResult = selectQueryHandler.Handle<TEntity>(selectQuery).ToList();
                int countResult = countQueryHandler.Handle<TEntity>(countQuery);
                QueryResponse response = new QueryResponse
                {
                    Value = selectResult,
                    Total = countResult,
                    Count = selectResult.Count
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
    }
}