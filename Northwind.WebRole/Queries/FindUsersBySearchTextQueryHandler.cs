using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Repositories;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class FindUsersBySearchTextQueryHandler : IQueryHandler<FindUsersBySearchTextQuery, User[]>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public FindUsersBySearchTextQueryHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User[] Handle(FindUsersBySearchTextQuery query)
        {
            //SqlDataAdapter adapter = _unitOfWork.Repository<User>().SqlDataAdapter();
            //dynamic parameters = new
            //{
            //    Username = query.SearchText,
            //    Active = !query.IncludeInactiveUsers
            //};
            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Username", query.SearchText},
                {"Active", !query.IncludeInactiveUsers}
            };

            //IList<string> columns = new List<string>
            //{
            //    "Username",
            //    "Active"
            //};

            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IQueryRepository<User> userRepository = _unitOfWork.QueryRepository<User>();
                IEnumerable<User> users = userRepository.Select(parameters);
                User[] array = users.ToArray();
                return array;
            }
        }

        public Task<User[]> HandleAsync(FindUsersBySearchTextQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}