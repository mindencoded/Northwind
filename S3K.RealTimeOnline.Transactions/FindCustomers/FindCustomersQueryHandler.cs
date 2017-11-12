using System.Collections.Generic;
using System.Dynamic;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.Transactions.FindCustomers
{
    public class FindCustomersQueryHandler : IQueryHandler<QueryBuilder, IEnumerable<ExpandoObject>>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public FindCustomersQueryHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExpandoObject> Handle(QueryBuilder query)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IEnumerable<ExpandoObject> result = _unitOfWork.Repository<Customer>().Select(
                    query.Columns,
                    query.Conditions,
                    query.OrderBy,
                    query.Page,
                    query.PageSize
                ) as IEnumerable<ExpandoObject>;
                return result;
            }
        }
    }
}
