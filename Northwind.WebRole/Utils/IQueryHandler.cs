﻿using System.Threading.Tasks;

namespace Northwind.WebRole.Utils
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);

        Task<TResult> HandleAsync(TQuery query);
    }
}